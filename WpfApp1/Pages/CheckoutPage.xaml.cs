using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

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

        /* ---------- итог корзины ---------- */
        private void LoadSummary()
        {
            int uid = App.CurrentUser.id;
            var ctx = AppConnect.OrganayzerRasteniyModel;

            decimal total = (from c in ctx.Cart.Where(x => x.user_id == uid)
                             join p in ctx.Plants on c.plant_id equals p.id
                             let disc = ctx.Discounts.FirstOrDefault(d => d.plant_id == p.id && d.start_date <= DateTime.Now && d.end_date >= DateTime.Now)
                             let price = (p.price ?? 0m) * (disc != null ? 1 - disc.discount_percent / 100m : 1m)
                             select price * c.quantity)
                            .DefaultIfEmpty(0m).Sum();

            TotalAmountText.Text = $"Итого: {total:N2} ₽";
        }

        /* ---------- справочники ---------- */
        private void LoadDeliveryMethods()
        {
            ComboDelivery.Items.Clear();
            ComboDelivery.Items.Add("Курьером");
            ComboDelivery.Items.Add("Самовывоз");
            ComboDelivery.SelectedIndex = 0;
        }

        private void LoadPaymentMethods()
        {
            // важно очистить Items, иначе конфликт Items / ItemsSource
            ComboPayment.Items.Clear();
            ComboPayment.Items.Add("Картой при получении");
            ComboPayment.Items.Add("Картой онлайн");
            ComboPayment.Items.Add("Наличными");
            ComboPayment.SelectedIndex = 0;
        }

        private string PaymentMethod => ComboPayment.SelectedItem as string;
        private string DeliveryMethod => ComboDelivery.SelectedItem as string;

        /* ---------- подтверждение заказа ---------- */
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
            if (cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }

            decimal total = (from c in cart
                             join p in ctx.Plants on c.plant_id equals p.id
                             let disc = ctx.Discounts.FirstOrDefault(d => d.plant_id == p.id && d.start_date <= DateTime.Now && d.end_date >= DateTime.Now)
                             let price = (p.price ?? 0m) * (disc != null ? 1 - disc.discount_percent / 100m : 1m)
                             select price * c.quantity).Sum();

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
            ctx.SaveChanges(); // получаем id

            foreach (var c in cart)
            {
                ctx.OrderDetails.Add(new OrderDetails
                {
                    order_id = order.id,
                    plant_id = c.plant_id,
                    quantity = c.quantity
                });
            }

            ctx.Payments.Add(new Payments
            {
                order_id = order.id,
                payment_date = DateTime.Now,
                amount = total,
                payment_method = payment,
                status = "Pending"
            });

            ctx.Cart.RemoveRange(cart);
            ctx.SaveChanges();

            MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new UserOrdersPage());
        }
    }
}
