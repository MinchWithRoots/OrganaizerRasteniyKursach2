using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class UserPlantsPage : Page
    {
        public UserPlantsPage()
        {
            InitializeComponent();
            LoadCategories();
            UpdatePlants();
        }

        private void LoadCategories()
        {
            var categories = AppConnect.OrganayzerRasteniyModel.Categories.Select(c => c.name).ToList();
            categories.Insert(0, "Все категории");
            ComboFilter.ItemsSource = categories;
            ComboFilter.SelectedIndex = 0;
        }

        private void UpdatePlants()
        {
            var plantsQuery = from p in AppConnect.OrganayzerRasteniyModel.Plants
                              join c in AppConnect.OrganayzerRasteniyModel.Categories on p.category_id equals c.id into gj
                              from subC in gj.DefaultIfEmpty()
                              select new
                              {
                                  Id = p.id,
                                  Name = p.name,
                                  Description = p.description,
                                  CategoryName = subC == null ? "Без категории" : subC.name,
                                  Price = p.price,
                              };

            // Фильтрация по категории
            string selectedCategory = ComboFilter.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Все категории")
                plantsQuery = plantsQuery.Where(p => p.CategoryName == selectedCategory);

            // Поиск
            string search = TextSearch.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
                plantsQuery = plantsQuery.Where(p => p.Name.ToLower().Contains(search) || p.Description.ToLower().Contains(search));

            // Сортировка
            var selectedItem = ComboSort.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string sortOption = selectedItem.Content.ToString();
                switch (sortOption)
                {
                    case "Название А-Я":
                        plantsQuery = plantsQuery.OrderBy(p => p.Name);
                        break;
                    case "Название Я-А":
                        plantsQuery = plantsQuery.OrderByDescending(p => p.Name);
                        break;
                    case "Цена ↑":
                        plantsQuery = plantsQuery.OrderBy(p => p.Price ?? 0);
                        break;
                    case "Цена ↓":
                        plantsQuery = plantsQuery.OrderByDescending(p => p.Price ?? 0);
                        break;
                }
            }

            var plants = plantsQuery.ToList();

            // Получаем активные скидки
            var activeDiscounts = AppConnect.OrganayzerRasteniyModel.Discounts
                .Where(d => d.start_date <= DateTime.Now && d.end_date >= DateTime.Now)
                .ToList();

            // Создаём список PlantViewItem
            var plantViewItems = new List<dynamic>();
            foreach (var item in plants)
            {
                var discount = activeDiscounts.FirstOrDefault(d => d.plant_id == item.Id);
                decimal? discountedPrice = null;
                bool hasActiveDiscount = false;
                decimal discountPercent = 0;
                string discountDescription = null;

                if (discount != null && item.Price.HasValue)
                {
                    hasActiveDiscount = true;
                    discountPercent = discount.discount_percent;
                    discountedPrice = item.Price.Value * (1 - (decimal)(discountPercent / 100));
                    discountDescription = discount.description;
                }

                plantViewItems.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CategoryName = item.CategoryName,
                    Price = item.Price,
                    PhotoPath = GetPhotoPath(item.Id),
                    HasActiveDiscount = hasActiveDiscount,
                    DiscountPercent = discountPercent,
                    DiscountedPrice = discountedPrice,
                    DiscountDescription = discountDescription
                });
            }

            ListPlants.ItemsSource = plantViewItems;
            CountRecords.Text = $"Найдено записей: {plantViewItems.Count}";
        }

        private string GetPhotoPath(int plantId)
        {
            var photo = AppConnect.OrganayzerRasteniyModel.Photos
                .Where(p => p.plant_id == plantId)
                .OrderByDescending(p => p.upload_date)
                .FirstOrDefault();
            return photo?.photo_path ?? "/Images/no-image.png";
        }

        // Обработчики событий
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlants();
        }

        private void ComboSort_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdatePlants();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlants();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            try
            {
                dynamic item = button.Tag;
                int plantId = item.Id;
                string plantName = item.Name;

                // Добавляем товар в корзину
                var cartItem = new Cart
                {
                    user_id = App.CurrentUser.id,
                    plant_id = plantId,
                    quantity = 1,
                    //added_date = DateTime.Now
                };

                AppConnect.OrganayzerRasteniyModel.Cart.Add(cartItem);
                AppConnect.OrganayzerRasteniyModel.SaveChanges();
                MessageBox.Show($"Растение '{plantName}' добавлено в корзину.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления в корзину: {ex.Message}");
            }
        }

        // Переходы на другие страницы
        private void GoToCart_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
        }

        private void GoToOrders_Click(object sender, MouseButtonEventArgs e)
        {
            //NavigationService.Navigate(new UserOrdersPage(App.CurrentUser.id));
        }

        private void GoToReminders_Click(object sender, MouseButtonEventArgs e)
        {
            //NavigationService.Navigate(new RemindersPage(App.CurrentUser.id));
        }

        private void GoToMyPlants_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new UserGardenPage());
        }
    }
}