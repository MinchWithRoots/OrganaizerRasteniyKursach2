using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

/* PDF + QR */
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using QRCoder;

namespace WpfApp1.Pages
{
    public partial class CheckoutPage : Page
    {
        public CheckoutPage()
        {
            InitializeComponent();
            LoadSummary();
            LoadDeliveryMethods();
            LoadPaymentMethods();
        }

        #region helpers-init
        private void LoadSummary()
        {
            int uid = App.CurrentUser.id;
            var ctx = AppConnect.OrganayzerRasteniyModel;

            decimal total =
                (from c in ctx.Cart.Where(x => x.user_id == uid)
                 join p in ctx.Plants on c.plant_id equals p.id
                 let disc = ctx.Discounts.FirstOrDefault(d => d.plant_id == p.id &&
                                                              d.start_date <= DateTime.Now &&
                                                              d.end_date >= DateTime.Now)
                 let price = (p.price ?? 0m) * (disc != null ? 1 - disc.discount_percent / 100m : 1m)
                 select price * c.quantity)
                .DefaultIfEmpty(0m).Sum();

            TotalAmountText.Text = $"Итого: {total:N2} ₽";
        }

        private void LoadDeliveryMethods()
        {
            ComboDelivery.Items.Clear();
            ComboDelivery.Items.Add("Курьером");
            ComboDelivery.Items.Add("Самовывоз");
            ComboDelivery.SelectedIndex = 0;
        }

        private void LoadPaymentMethods()
        {
            ComboPayment.Items.Clear();
            ComboPayment.Items.Add("Картой при получении");
            ComboPayment.Items.Add("Картой онлайн");
            ComboPayment.Items.Add("Наличными");
            ComboPayment.SelectedIndex = 0;
        }
        #endregion

        private string PaymentMethod => ComboPayment.SelectedItem as string;
        private string DeliveryMethod => ComboDelivery.SelectedItem as string;

        #region confirm
        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            string address = TextAddress.Text.Trim();
            string payment = PaymentMethod;
            string delivery = DeliveryMethod;
            string comment = TextComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Введите адрес доставки.");
                return;
            }

            int uid = App.CurrentUser.id;
            var ctx = AppConnect.OrganayzerRasteniyModel;
            var cart = ctx.Cart.Where(c => c.user_id == uid).ToList();
            if (!cart.Any())
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }

            /* сумма заказа */
            decimal total =
                (from c in cart
                 join p in ctx.Plants on c.plant_id equals p.id
                 let disc = ctx.Discounts.FirstOrDefault(d => d.plant_id == p.id &&
                                                              d.start_date <= DateTime.Now &&
                                                              d.end_date >= DateTime.Now)
                 let price = (p.price ?? 0m) * (disc != null ? 1 - disc.discount_percent / 100m : 1m)
                 select price * c.quantity).Sum();

            /* создаём заказ */
            var order = new Orders
            {
                user_id = uid,
                order_date = DateTime.Now,
                total_amount = total,
                status = "Ожидает оплаты",
                shipping_address = address,
                payment_method = payment,
                delivery_method = delivery,
                comment = comment
            };
            ctx.Orders.Add(order);
            ctx.SaveChanges(); // нужен order.id

            /* ---------- детали + добавление в UserPlants ---------- */
            foreach (var item in cart)          // ← имя переменной изменено
            {
                /* деталь заказа (как было) */
                var plant = ctx.Plants.First(p => p.id == item.plant_id);
                decimal eachPrice = plant.price ?? 0m;
                var disc = ctx.Discounts.FirstOrDefault(d => d.plant_id == plant.id &&
                                                             d.start_date <= DateTime.Now &&
                                                             d.end_date >= DateTime.Now);
                if (disc != null)
                    eachPrice *= 1 - disc.discount_percent / 100m;

                ctx.OrderDetails.Add(new OrderDetails
                {
                    order_id = order.id,
                    plant_id = item.plant_id,
                    quantity = item.quantity,
                    price = eachPrice
                });

                /* ---- ДОБАВЛЯЕМ В МОИ РАСТЕНИЯ построчно, без «уходовых» полей ---- */
                for (int i = 0; i < item.quantity; i++)
                {
                    ctx.UserPlants.Add(new UserPlants
                    {
                        user_id = uid,
                        plant_id = item.plant_id,
                        purchase_date = DateTime.Now,
                        /* nothing else – CareSchedule / Notes / Room и др. остаются NULL,
                           поэтому в UserGardenPage они НЕ будут отображаться */
                    });
                }
            }


            /* платёж-заглушка */
            ctx.Payments.Add(new Payments
            {
                order_id = order.id,
                payment_date = DateTime.Now,
                amount = total,
                payment_method = payment,
                status = "Pending"
            });

            /* очистка корзины */
            ctx.Cart.RemoveRange(cart);
            ctx.SaveChanges();

            /* чек + QR + автопоказ */
            string pdfPath = GenerateReceiptPdfWithQr(order, cart);
            TryOpen(pdfPath);

            MessageBox.Show("Заказ успешно оформлен!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new UserOrdersPage());
        }
        #endregion

        #region PDF-receipt + QR
        private string GenerateReceiptPdfWithQr(Orders order, System.Collections.Generic.List<Cart> cart)
        {
            var ctx = AppConnect.OrganayzerRasteniyModel;
            var plants = ctx.Plants.ToList();

            string dir = Path.Combine(Environment.CurrentDirectory, "Receipts");
            Directory.CreateDirectory(dir);
            string pdfPath = Path.Combine(dir, $"Receipt_Order_{order.id}.pdf");

            /* ---------- формируем QR-текст (вся инфа как в чеке) ---------- */
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Чек заказа №{order.id}");
            sb.AppendLine($"Дата: {order.order_date:g}");
            sb.AppendLine($"Адрес: {order.shipping_address}");
            sb.AppendLine($"Оплата: {order.payment_method}");
            sb.AppendLine($"Доставка: {order.delivery_method}");
            sb.AppendLine("Товары:");
            foreach (var c in cart)
            {
                var pl = plants.First(p => p.id == c.plant_id);
                decimal each = pl.price ?? 0m;
                sb.AppendLine($" • {pl.name} × {c.quantity} — {(each * c.quantity):N2} ₽");
            }
            sb.AppendLine($"Итого: {order.total_amount:N2} ₽");
            sb.AppendLine($"Код: ORD-{order.id:D6}");
            string qrText = sb.ToString();

            /* создаём QR-код */
            Bitmap qrBmp;
            using (var gen = new QRCodeGenerator())
            {
                var qrData = gen.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                using (var qr = new QRCode(qrData))
                    qrBmp = qr.GetGraphic(20);
            }

            /* ---------- создаём PDF ---------- */
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            XGraphics g = XGraphics.FromPdfPage(page);

            XFont fBold = new XFont("Arial", 14, XFontStyle.Bold);
            XFont fReg = new XFont("Arial", 10, XFontStyle.Regular);

            /* заголовок */
            double y = 40;
            g.DrawString($"ЧЕК ЗАКАЗА №{order.id}", fBold, XBrushes.Black,
                         new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 30;

            void L(string t) { g.DrawString(t, fReg, XBrushes.Black, 40, y); y += 15; }

            L($"Дата: {order.order_date:g}");
            L($"Адрес: {order.shipping_address}");
            L($"Оплата: {order.payment_method}");
            L($"Доставка: {order.delivery_method}");
            y += 5;
            g.DrawString("Товары:", fBold, XBrushes.Black, 40, y); y += 18;

            foreach (var c in cart)
            {
                var pl = plants.First(p => p.id == c.plant_id);
                decimal sum = (pl.price ?? 0m) * c.quantity;
                L($"• {pl.name} × {c.quantity} — {sum:N2} ₽");
            }
            y += 6;
            g.DrawString($"Итого: {order.total_amount:N2} ₽", fBold, XBrushes.Black, 40, y);

            /* вставляем QR */
            using (var ms = new MemoryStream())
            {
                qrBmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                XImage qrImg = XImage.FromStream(ms);

                double qrSize = 110;
                double xQr = page.Width - qrSize - 40;
                double yQr = 40;
                g.DrawImage(qrImg, xQr, yQr, qrSize, qrSize);
            }

            doc.Save(pdfPath);
            return pdfPath;
        }

        private static void TryOpen(string file)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = file,
                    UseShellExecute = true
                });
            }
            catch { /* ignore */ }
        }
        #endregion

        /* кнопка Назад */
        private void BackToCatalog_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new UserPlantsPage());
    }
}
