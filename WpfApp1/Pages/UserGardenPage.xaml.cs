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
    public partial class UserGardenPage : Page
    {
        private object _context;

        // Конструктор без параметров
        public UserGardenPage() : this(null)
        {
        }

        // Конструктор с контекстом (можно использовать для передачи данных)
        public UserGardenPage(object context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
            UpdateUserPlants();

            if (NavigationService != null)
            {
                NavigationService.Navigated += (sender, e) =>
                {
                    if (e.Content == this)
                        UpdateUserPlants();
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

        private void UpdateUserPlants()
        {
            int userId = App.CurrentUser.id;

            var plantsQuery = from up in AppConnect.OrganayzerRasteniyModel.UserPlants
                              where up.user_id == userId
                              join p in AppConnect.OrganayzerRasteniyModel.Plants on up.plant_id equals p.id
                              join c in AppConnect.OrganayzerRasteniyModel.Categories on p.category_id equals c.id into gj
                              from subC in gj.DefaultIfEmpty()

                                  // Присоединяем температурные предпочтения
                              join temp in AppConnect.OrganayzerRasteniyModel.Temperature on p.id equals temp.plant_id into tempj
                              from subTemp in tempj.DefaultIfEmpty()

                                  // Присоединяем полив
                              join water in AppConnect.OrganayzerRasteniyModel.Watering on p.id equals water.plant_id into waterj
                              from subWater in waterj.DefaultIfEmpty()

                                  // Присоединяем подкормку
                              join fert in AppConnect.OrganayzerRasteniyModel.Fertilization on p.id equals fert.plant_id into fertj
                              from subFert in fertj.DefaultIfEmpty()

                                  // Присоединяем удобрения
                              join f in AppConnect.OrganayzerRasteniyModel.Fertilizers on subFert.fertilizer_id equals f.id into fertilizersGroup
                              from subFertName in fertilizersGroup.DefaultIfEmpty()

                                  // Присоединяем освещение
                              join light in AppConnect.OrganayzerRasteniyModel.Lighting on p.id equals light.plant_id into lightj
                              from subLight in lightj.DefaultIfEmpty()

                                  // Присоединяем расположение растения
                              join pl in AppConnect.OrganayzerRasteniyModel.PlantLocations on p.id equals pl.plant_id into plj
                              from subPl in plj.DefaultIfEmpty()

                                  // Присоединяем комнаты
                              join r in AppConnect.OrganayzerRasteniyModel.Rooms on subPl.room_id equals r.id into rj
                              from subR in rj.DefaultIfEmpty()
                              select new
                              {
                                  Id = up.plant_id,
                                  Name = p.name,
                                  Description = p.description,
                                  CategoryName = subC == null ? "Без категории" : subC.name,

                                  PurchaseDate = up.purchase_date,
                                  LastCareDate = up.last_care_date,
                                  CareSchedule = up.care_schedule,
                                  Notes = up.notes,

                                  MinTemp = subTemp == null ? (decimal?)null : subTemp.min_temp,
                                  MaxTemp = subTemp == null ? (decimal?)null : subTemp.max_temp,

                                  WateringAmount = subWater == null ? (int?)null : subWater.amount_ml,

                                  FertilizerName = subFertName == null ? null : subFertName.name,
                                  FertilizerDosage = subFert == null ? (int?)null : subFert.dosage_ml,

                                  LightingLevel = subLight == null ? "Не указано" : subLight.level,
                                  UsesLights = subLight == null ? (bool?)null : subLight.uses_lights,

                                  RoomName = subR == null ? null : subR.name,
                                  WindowPosition = subPl == null ? null : subPl.window_position
                              };

            // Фильтр по категории
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
                switch (selectedItem.Content.ToString())
                {
                    case "Название А-Я":
                        plantsQuery = plantsQuery.OrderBy(p => p.Name);
                        break;
                    case "Название Я-А":
                        plantsQuery = plantsQuery.OrderByDescending(p => p.Name);
                        break;
                    case "Дата ухода ↑":
                        plantsQuery = plantsQuery.OrderBy(p => p.LastCareDate);
                        break;
                    case "Дата ухода ↓":
                        plantsQuery = plantsQuery.OrderByDescending(p => p.LastCareDate);
                        break;
                }
            }

            var result = plantsQuery.ToList();

            var plantViewItems = new List<PlantViewItem>();
            foreach (var item in result)
            {
                string photoPath = GetPhotoPath(item.Id);

                plantViewItems.Add(new PlantViewItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CategoryName = item.CategoryName,
                    PurchaseDate = item.PurchaseDate,
                    LastCareDate = item.LastCareDate,
                    CareSchedule = item.CareSchedule,
                    Notes = item.Notes,

                    MinTemp = item.MinTemp,
                    MaxTemp = item.MaxTemp,
                    WateringAmount = item.WateringAmount,
                    FertilizerName = item.FertilizerName ?? "Без удобрений",
                    FertilizerDosage = item.FertilizerDosage,
                    LightingLevel = item.LightingLevel,
                    UsesLights = item.UsesLights,
                    RoomName = item.RoomName,
                    WindowPosition = item.WindowPosition,

                    PhotoPath = photoPath
                });
            }

            ListUserPlants.ItemsSource = plantViewItems;
            CountRecords.Text = $"Растений: {plantViewItems.Count}";
        }

        private static int? ParseIntOrNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (int.TryParse(value, out int result))
                return result;

            return null;
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

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUserPlants();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUserPlants();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUserPlants();
        }

        private void bCreatePlant_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateUserPlantPage());
        }

        private PlantViewItem GetPlantViewItem(int userPlantId)
        {
            // Найдите UserPlants запись
            var userPlant = AppConnect.OrganayzerRasteniyModel.UserPlants.Find(userPlantId);
            if (userPlant == null)
            {
                return null;
            }

            // Получите общую информацию о растении
            var plant = AppConnect.OrganayzerRasteniyModel.Plants.FirstOrDefault(p => p.id == userPlant.plant_id);

            if (plant == null)
            {
                return null;
            }

            // Соберите все необходимые данные
            var plantViewItem = new PlantViewItem
            {
                Id = plant.id,
                Name = plant.name,
                Description = plant.description,
                CategoryName = AppConnect.OrganayzerRasteniyModel.Categories
                    .FirstOrDefault(c => c.id == plant.category_id)?.name ?? "Без категории",
                PurchaseDate = userPlant.purchase_date,
                LastCareDate = userPlant.last_care_date,
                CareSchedule = userPlant.care_schedule,
                Notes = userPlant.notes,

                // Температура
                MinTemp = AppConnect.OrganayzerRasteniyModel.Temperature
                    .FirstOrDefault(t => t.plant_id == plant.id)?.min_temp,
                MaxTemp = AppConnect.OrganayzerRasteniyModel.Temperature
                    .FirstOrDefault(t => t.plant_id == plant.id)?.max_temp,

                // Полив
                WateringAmount = AppConnect.OrganayzerRasteniyModel.Watering
                    .FirstOrDefault(w => w.plant_id == plant.id)?.amount_ml,

                // Подкормка
                FertilizerName = AppConnect.OrganayzerRasteniyModel.Fertilization
                    .Join(AppConnect.OrganayzerRasteniyModel.Fertilizers,
                          f => f.fertilizer_id,
                          f_name => f_name.id,
                          (f, f_name) => new { Fertilizer = f, Name = f_name.name })
                    .FirstOrDefault(f => f.Fertilizer.plant_id == plant.id)?.Name,
                FertilizerDosage = AppConnect.OrganayzerRasteniyModel.Fertilization
                    .FirstOrDefault(f => f.plant_id == plant.id)?.dosage_ml,

                // Освещение
                LightingLevel = AppConnect.OrganayzerRasteniyModel.Lighting
                    .FirstOrDefault(l => l.plant_id == plant.id)?.level,
                UsesLights = AppConnect.OrganayzerRasteniyModel.Lighting
                    .FirstOrDefault(l => l.plant_id == plant.id)?.uses_lights,

                // Расположение
                RoomName = AppConnect.OrganayzerRasteniyModel.PlantLocations
                    .Join(AppConnect.OrganayzerRasteniyModel.Rooms,
                          pl => pl.room_id,
                          r => r.id,
                          (pl, room) => new { Location = pl, Room = room })
                    .FirstOrDefault(pl => pl.Location.plant_id == plant.id)?.Room.name,
                WindowPosition = AppConnect.OrganayzerRasteniyModel.PlantLocations
                    .FirstOrDefault(pl => pl.plant_id == plant.id)?.window_position,

                // Фото
                PhotoPath = GetPhotoPath(plant.id)
            };

            return plantViewItem;
        }

        private void EditUserPlant_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            dynamic item = button.Tag;
            int userPlantId = item.Id;

            var dbItem = AppConnect.OrganayzerRasteniyModel.UserPlants.Find(userPlantId);
            if (dbItem == null)
            {
                MessageBox.Show("Растение не найдено.");
                return;
            }

            // Получите полные данные о растении
            var plantViewItem = GetPlantViewItem(userPlantId);

            if (plantViewItem == null)
            {
                MessageBox.Show("Не удалось загрузить данные о растении.");
                return;
            }

            // Переход на страницу редактирования растения
            NavigationService.Navigate(new CreateUserPlantPage(plantViewItem));
        }

        private void DeleteUserPlant_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            dynamic item = button.Tag;
            int userPlantId = item.Id;
            string plantName = item.Name;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить растение '{plantName}'?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dbItem = AppConnect.OrganayzerRasteniyModel.UserPlants.Find(userPlantId);
                    if (dbItem != null)
                    {
                        AppConnect.OrganayzerRasteniyModel.UserPlants.Remove(dbItem);
                        AppConnect.OrganayzerRasteniyModel.SaveChanges();
                        UpdateUserPlants();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении растения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GoToNotifications_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(App.CurrentUser.id));
        }
    }

    
}