using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class DiscountsPage : Page
    {
        public DiscountsPage()
        {
            InitializeComponent();
            LoadDiscounts();
        }

        private void LoadDiscounts()
        {
            var discounts = from d in AppConnect.OrganayzerRasteniyModel.Discounts
                            join p in AppConnect.OrganayzerRasteniyModel.Plants on d.plant_id equals p.id
                            select new DiscountViewItem
                            {
                                Id = d.id,
                                PlantId = p.id,
                                PlantName = p.name,
                                DiscountPercent = d.discount_percent,
                                Description = d.description,
                                StartDate = d.start_date,
                                EndDate = d.end_date,
                                Color = d.color
                            };

            ListDiscounts.ItemsSource = discounts.ToList();
        }

        private void AddDiscount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditDiscountPage());
        }

        private void EditDiscount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is DiscountViewItem discount)
            {
                NavigationService.Navigate(new EditDiscountPage(discount.Id));
            }
        }

        private void DeleteDiscount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is DiscountViewItem discount)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить скидку на '{discount.PlantName}'?",
                                             "Подтверждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var dbDiscount = AppConnect.OrganayzerRasteniyModel.Discounts.Find(discount.Id);
                        if (dbDiscount != null)
                        {
                            AppConnect.OrganayzerRasteniyModel.Discounts.Remove(dbDiscount);
                            AppConnect.OrganayzerRasteniyModel.SaveChanges();
                            LoadDiscounts(); // обновляем список
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении скидки: {ex.Message}");
                    }
                }
            }
        }

        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPlantsPage());
        }
    }

    public class DiscountViewItem
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public decimal DiscountPercent { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Color { get; set; } = "#FFD700"; // по умолчанию золотой
    }
}