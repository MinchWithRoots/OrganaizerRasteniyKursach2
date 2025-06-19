using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace WpfApp1.Pages
{
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            int userId = App.CurrentUser.id;

            var cartItems = from c in AppConnect.OrganayzerRasteniyModel.Cart
                            where c.user_id == userId
                            join p in AppConnect.OrganayzerRasteniyModel.Plants on c.plant_id equals p.id
                            select new { c, p };

            var activeDiscounts = AppConnect.OrganayzerRasteniyModel.Discounts
                .Where(d => d.start_date <= DateTime.Now && d.end_date >= DateTime.Now)
                .ToList();

            var result = new List<CartItem>();

            foreach (var item in cartItems)
            {
                var discount = activeDiscounts.FirstOrDefault(d => d.plant_id == item.p.id);
                decimal basePrice = item.p.price ?? 0;
                decimal finalPrice = basePrice;
                decimal? discountPercent = null;
                string discountText = null;

                if (discount != null)
                {
                    discountPercent = discount.discount_percent;
                    finalPrice = basePrice * (1 - discountPercent.Value / 100);
                    discountText = discount.description;
                }

                result.Add(new CartItem
                {
                    PlantId = item.p.id,
                    Name = item.p.name,
                    Price = basePrice,
                    FinalPrice = finalPrice,
                    Quantity = item.c.quantity,
                    DiscountPercent = discountPercent,
                    DiscountDescription = discountText,
                    Sum = finalPrice * item.c.quantity,
                    PhotoPath = GetPhoto(item.p.id),
                    ShowDiscount = discountPercent.HasValue
                });
            }

            ListCart.ItemsSource = result;
            TotalItemsText.Text = $"Товаров: {result.Sum(x => x.Quantity)}";
            TotalSumText.Text = $"Сумма: {result.Sum(x => x.Sum):N2} ₽";
        }

        private string GetPhoto(int plantId)
        {
            var photo = AppConnect.OrganayzerRasteniyModel.Photos
                .Where(p => p.plant_id == plantId)
                .OrderByDescending(p => p.upload_date)
                .FirstOrDefault();

            return photo?.photo_path ?? "/Images/no-image.png";
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is CartItem item)
            {
                var cart = AppConnect.OrganayzerRasteniyModel.Cart
                    .FirstOrDefault(x => x.user_id == App.CurrentUser.id && x.plant_id == item.PlantId);

                if (cart != null)
                {
                    cart.quantity++;
                    AppConnect.OrganayzerRasteniyModel.SaveChanges();
                    LoadCart();
                }
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is CartItem item)
            {
                var cart = AppConnect.OrganayzerRasteniyModel.Cart
                    .FirstOrDefault(x => x.user_id == App.CurrentUser.id && x.plant_id == item.PlantId);

                if (cart != null)
                {
                    if (cart.quantity > 1)
                        cart.quantity--;
                    else
                        AppConnect.OrganayzerRasteniyModel.Cart.Remove(cart);

                    AppConnect.OrganayzerRasteniyModel.SaveChanges();
                    LoadCart();
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (CartItem)((Button)sender).Tag; var cart = AppConnect.OrganayzerRasteniyModel.Cart
                .FirstOrDefault(c => c.plant_id == item.PlantId && c.user_id == App.CurrentUser.id); if (cart != null)
            {
                AppConnect.OrganayzerRasteniyModel.Cart.Remove(cart);
                AppConnect.OrganayzerRasteniyModel.SaveChanges(); LoadCart();
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // переходим на страницу оформления
            NavigationService.Navigate(new CheckoutPage());
        }
    }

    public class CartItem
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Sum { get; set; }
        public string PhotoPath { get; set; }
        public decimal? DiscountPercent { get; set; }
        public string DiscountDescription { get; set; }
        public bool ShowDiscount { get; set; }
    }
}