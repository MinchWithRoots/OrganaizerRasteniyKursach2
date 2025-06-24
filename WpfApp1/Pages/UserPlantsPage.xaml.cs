using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.IO;
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

        /* ───────────────────── справочники ───────────────────── */
        private void LoadCategories()
        {
            var categories = AppConnect.OrganayzerRasteniyModel.Categories
                             .Select(c => c.name).ToList();
            categories.Insert(0, "Все категории");

            ComboFilter.ItemsSource = categories;
            ComboFilter.SelectedIndex = 0;
        }

        /* ───────────────────── выборка каталога ───────────────────── */
        private void UpdatePlants()
        {
            /* берём только позиции, у которых price IS NOT NULL  */
            var plantsQuery =
                from p in AppConnect.OrganayzerRasteniyModel.Plants
                where p.price.HasValue
                join c in AppConnect.OrganayzerRasteniyModel.Categories
                     on p.category_id equals c.id into gj
                from subC in gj.DefaultIfEmpty()
                select new
                {
                    Id = p.id,
                    Name = p.name,
                    Description = p.description,
                    CategoryName = subC == null ? "Без категории" : subC.name,
                    Price = p.price
                };

            /* ── фильтр по категории ── */
            string selectedCategory = ComboFilter.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedCategory) &&
                selectedCategory != "Все категории")
                plantsQuery = plantsQuery.Where(p => p.CategoryName == selectedCategory);

            /* ── поиск ── */
            string search = TextSearch.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
                plantsQuery = plantsQuery.Where(p =>
                               p.Name.ToLower().Contains(search) ||
                               p.Description.ToLower().Contains(search));

            /* ── сортировка ── */
            var sortItem = ComboSort.SelectedItem as ComboBoxItem;
            if (sortItem != null)
            {
                switch (sortItem.Content.ToString())
                {
                    case "Название А-Я": plantsQuery = plantsQuery.OrderBy(p => p.Name); break;
                    case "Название Я-А": plantsQuery = plantsQuery.OrderByDescending(p => p.Name); break;
                    case "Цена ↑": plantsQuery = plantsQuery.OrderBy(p => p.Price ?? 0); break;
                    case "Цена ↓": plantsQuery = plantsQuery.OrderByDescending(p => p.Price ?? 0); break;
                }
            }

            var plants = plantsQuery.ToList();

            /* ── активные скидки ── */
            var discounts = AppConnect.OrganayzerRasteniyModel.Discounts
                            .Where(d => d.start_date <= DateTime.Now &&
                                        d.end_date >= DateTime.Now)
                            .ToList();

            /* ── view-list ── */
            var viewList = new List<dynamic>();
            foreach (var item in plants)
            {
                var disc = discounts.FirstOrDefault(d => d.plant_id == item.Id);

                bool hasDisc = disc != null;
                decimal discPercent = hasDisc ? disc.discount_percent : 0;
                decimal? newPrice = hasDisc
                                      ? item.Price.Value * (1 - discPercent / 100m)
                                      : (decimal?)null;

                viewList.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CategoryName = item.CategoryName,
                    Price = item.Price,
                    PhotoPath = GetPhotoPath(item.Id),
                    HasActiveDiscount = hasDisc,
                    DiscountPercent = discPercent,
                    DiscountedPrice = newPrice,
                    DiscountDescription = hasDisc ? disc.description : null
                });
            }

            ListPlants.ItemsSource = viewList;
            CountRecords.Text = $"Найдено записей: {viewList.Count}";
        }

        private string GetPhotoPath(int plantId)
        {
            var photo = AppConnect.OrganayzerRasteniyModel.Photos
                         .Where(p => p.plant_id == plantId)
                         .OrderByDescending(p => p.upload_date)
                         .FirstOrDefault();

            if (photo != null && !string.IsNullOrEmpty(photo.photo_path))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photo.photo_path.TrimStart('/'));
                if (File.Exists(fullPath))
                    return fullPath;
            }

            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "no-image.png");
            if (!File.Exists(defaultImagePath))
            {
                return "pack://application:,,,/Images/no-image.jpg";
            }

            return defaultImagePath;
        }


        /* ───────────────────── события UI ───────────────────── */
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdatePlants();
        private void ComboSort_SelectionChanged(object sender, RoutedEventArgs e) => UpdatePlants();
        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdatePlants();

        /* ───────────────────── добавление в корзину ───────────────────── */
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null || btn.Tag == null) return;

            dynamic vm = btn.Tag;        // объект из ItemsSource
            int plantId = vm.Id;
            string plantName = vm.Name;

            var ctx = AppConnect.OrganayzerRasteniyModel;

            try
            {
                /* 1) проверяем товар */
                var plant = ctx.Plants.FirstOrDefault(p => p.id == plantId);
                if (plant == null || !plant.price.HasValue)
                {
                    MessageBox.Show("Эта позиция недоступна для покупки.");
                    return;
                }

                int userId = App.CurrentUser.id;

                /* 2) ищем строку корзины */
                var cartRow = ctx.Cart.FirstOrDefault(c =>
                                c.user_id == userId &&
                                c.plant_id == plantId);

                if (cartRow == null)          // ещё нет — создаём
                {
                    cartRow = new Cart
                    {
                        user_id = userId,
                        plant_id = plantId,
                        quantity = 1,
                        added_date = DateTime.Now  // обязательное поле даты
                    };
                    ctx.Cart.Add(cartRow);
                }
                else                           // есть — увеличиваем количество
                {
                    cartRow.quantity++;
                }

                ctx.SaveChanges();

                MessageBox.Show($"«{plantName}» добавлен в корзину (кол-во: {cartRow.quantity}).",
                                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                string msg = dbEx.InnerException != null
                           ? dbEx.InnerException.InnerException?.Message ?? dbEx.InnerException.Message
                           : dbEx.Message;

                MessageBox.Show("Не удалось добавить товар: " + msg,
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить товар: " + ex.Message,
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /* ───────────────────── навигация ───────────────────── */
        private void GoToCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
        }

        private void GoToOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserOrdersPage());
        }

        private void GoToReminders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(App.CurrentUser.id));
        }

        private void GoToMyPlants_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserGardenPage());
        }

    }
}