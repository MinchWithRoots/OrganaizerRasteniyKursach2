using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class AdminPlantsPage : Page
    {
        public AdminPlantsPage()
        {
            InitializeComponent();
            LoadCategories();
            UpdatePlants();

            // Обработка навигации
            if (NavigationService != null)
            {
                NavigationService.Navigated += (sender, e) =>
                {
                    if (e.Content == this)
                    {
                        UpdatePlants(); // Обновляем данные при возврате на страницу
                    }
                };
            }
        }

        private void LoadCategories()
        {
            var categories = AppConnect.OrganayzerRasteniyModel.Categories.Select(c => c.name).ToList();
            categories.Insert(0, "Все категории");
            ComboFilter.ItemsSource = categories;
            ComboFilter.SelectedIndex = 0;
        }

        private void UpdatePlants(bool forceReloadPhotos = false)
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
                string photoPath = GetPhotoPath(item.Id);
                if (string.IsNullOrEmpty(photoPath))
                {
                    photoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "no-image.png");
                }

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
                    PhotoPath = photoPath,
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

            if (photo != null && !string.IsNullOrEmpty(photo.photo_path))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photo.photo_path.TrimStart('/'));
                if (File.Exists(fullPath))
                    return fullPath;
            }

            // Дефолтное изображение
            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "no-image.png");
            if (!File.Exists(defaultImagePath))
            {
                // Если no-image.png не найден, используем встроенный ресурс
                return "pack://application:,,,/Images/no-image.jpg";
            }

            return defaultImagePath;
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlants();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlants();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlants();
        }

        private void EditPlant_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            try
            {
                dynamic item = button.Tag;
                int plantId = item.Id;

                var plant = AppConnect.OrganayzerRasteniyModel.Plants.FirstOrDefault(p => p.id == plantId);
                if (plant == null)
                {
                    MessageBox.Show("Растение не найдено.");
                    return;
                }

                // Переход на страницу редактирования
                NavigationService.Navigate(new CreatePlantPage(plant));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных о растении: {ex.Message}");
            }
        }

      
       

        private void DeletePlant_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag != null)
            {
                dynamic item = button.Tag;

                int plantId;
                try
                {
                    plantId = item.Id;
                }
                catch
                {
                    MessageBox.Show("Не удалось получить ID растения.");
                    return;
                }

                string plantName = "неизвестное растение";
                try
                {
                    plantName = item.Name ?? "растение";
                }
                catch { }

                var result = MessageBox.Show($"Вы уверены, что хотите удалить растение '{plantName}'?",
                                             "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var dbPlant = AppConnect.OrganayzerRasteniyModel.Plants.Find(plantId);
                        if (dbPlant != null)
                        {
                            AppConnect.OrganayzerRasteniyModel.Plants.Remove(dbPlant);
                            AppConnect.OrganayzerRasteniyModel.SaveChanges();
                            UpdatePlants();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении растения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void bCreatePlant_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreatePlantPage());
        }

        private void ProfileIcon_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new PersonalAccountPage());
        }

        private void GoToDiscounts_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new DiscountsPage());
        }
        private void GoToOrders_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

    }

    public class PlantViewItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? LastCareDate { get; set; }
        public string CareSchedule { get; set; }
        public string Notes { get; set; }

        public decimal? MinTemp { get; set; }
        public decimal? MaxTemp { get; set; }
        public int? WateringAmount { get; set; }
        public string FertilizerName { get; set; }
        public int? FertilizerDosage { get; set; }
        public string LightingLevel { get; set; }
        public bool? UsesLights { get; set; }
        public string RoomName { get; set; }
        public string WindowPosition { get; set; }

        public string PhotoPath { get; set; }
    }
}