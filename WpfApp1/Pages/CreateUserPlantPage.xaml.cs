using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class CreateUserPlantPage : System.Windows.Controls.Page
    {
        private PlantViewItem _plantItem;
        private bool _isNew = true;
        private string _coverImagePath = null;

        public CreateUserPlantPage() : this(null) { }

        public CreateUserPlantPage(PlantViewItem plantItem)
        {
            InitializeComponent();

            if (plantItem != null)
            {
                _plantItem = plantItem;
                _isNew = false;

                // Заполняем поля данными для редактирования
                txtName.Text = _plantItem.Name;
                txtDescription.Text = _plantItem.Description;
                dpPurchaseDate.SelectedDate = _plantItem.PurchaseDate;
                dpLastCareDate.SelectedDate = _plantItem.LastCareDate;
                txtCareSchedule.Text = _plantItem.CareSchedule;
                txtNotes.Text = _plantItem.Notes;
                txtMinTemp.Text = _plantItem.MinTemp?.ToString();
                txtMaxTemp.Text = _plantItem.MaxTemp?.ToString();
                txtWateringAmount.Text = _plantItem.WateringAmount?.ToString();
                txtFertilizerDosage.Text = _plantItem.FertilizerDosage?.ToString();
                cbLightingLevel.SelectedItem = _plantItem.LightingLevel;
                cbUsesLights.IsChecked = _plantItem.UsesLights;
                txtWindowPosition.Text = _plantItem.WindowPosition;

                // Загружаем фото
                if (!string.IsNullOrEmpty(_plantItem.PhotoPath))
                {
                    imgPreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_plantItem.PhotoPath));
                }
            }
            else
            {
                _plantItem = new PlantViewItem
                {
                    PurchaseDate = DateTime.Today,
                    LastCareDate = DateTime.Today
                };
            }

            LoadDefaultData();
        }

        private void LoadDefaultData()
        {
            cbCategory.ItemsSource = AppConnect.OrganayzerRasteniyModel.Categories.Select(c => c.name).ToList();
            cbFertilizer.ItemsSource = AppConnect.OrganayzerRasteniyModel.Fertilizers.Select(f => f.name).ToList();
            cbRoom.ItemsSource = AppConnect.OrganayzerRasteniyModel.Rooms.Select(r => r.name).ToList();

            if (_plantItem != null)
            {
                cbCategory.SelectedItem = _plantItem.CategoryName;
                cbFertilizer.SelectedItem = _plantItem.FertilizerName;
                cbRoom.SelectedItem = _plantItem.RoomName;
            }
        }

        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                _coverImagePath = openFileDialog.FileName;
                imgPreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_coverImagePath));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Считываем данные с формы
                string name = txtName.Text.Trim();
                string description = txtDescription.Text.Trim();
                int? categoryId = cbCategory.SelectedValue as int?;
                DateTime purchaseDate = dpPurchaseDate.SelectedDate ?? DateTime.Now;
                DateTime? lastCareDate = dpLastCareDate.SelectedDate;
                string careSchedule = txtCareSchedule.Text.Trim();
                string notes = txtNotes.Text.Trim();
                decimal? minTemp = decimal.TryParse(txtMinTemp.Text, out decimal temp) ? temp : (decimal?)null;
                decimal? maxTemp = decimal.TryParse(txtMaxTemp.Text, out temp) ? temp : (decimal?)null;
                int? wateringAmount = int.TryParse(txtWateringAmount.Text, out int wa) ? wa : (int?)null;
                int? fertilizerDosage = int.TryParse(txtFertilizerDosage.Text, out int fd) ? fd : (int?)null;
                string lightingLevel = cbLightingLevel.SelectedItem?.ToString();
                bool? usesLights = cbUsesLights.IsChecked;
                string roomName = cbRoom.SelectedItem?.ToString();
                string windowPosition = txtWindowPosition.Text.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Введите название растения.");
                    return;
                }

                Plants plant;

                if (_isNew)
                {
                    // Создаем новое растение
                    plant = new Plants
                    {
                        name = name,
                        description = description,
                        category_id = categoryId
                    };

                    AppConnect.OrganayzerRasteniyModel.Plants.Add(plant);
                }
                else
                {
                    // Обновляем существующее растение
                    plant = AppConnect.OrganayzerRasteniyModel.Plants.Find(_plantItem.Id);

                    if (plant == null)
                    {
                        MessageBox.Show("Растение не найдено.");
                        return;
                    }

                    plant.name = name;
                    plant.description = description;
                    plant.category_id = categoryId;
                }

                AppConnect.OrganayzerRasteniyModel.SaveChanges(); // Сохраняем, чтобы получить ID нового растения

                if (_isNew)
                {
                    _plantItem.Id = plant.id; // Устанавливаем ID созданного растения
                    _isNew = false; // Теперь это не новый объект
                }

                // Добавляем/обновляем запись в UserPlants для текущего пользователя
                var currentUserPlant = AppConnect.OrganayzerRasteniyModel.UserPlants.FirstOrDefault(up => up.user_id == App.CurrentUser.id && up.plant_id == plant.id);

                if (currentUserPlant == null)
                {
                    currentUserPlant = new UserPlants
                    {
                        user_id = App.CurrentUser.id,
                        plant_id = plant.id,
                        purchase_date = purchaseDate,
                        last_care_date = lastCareDate,
                        care_schedule = careSchedule,
                        notes = notes
                    };

                    AppConnect.OrganayzerRasteniyModel.UserPlants.Add(currentUserPlant);
                }
                else
                {
                    currentUserPlant.purchase_date = purchaseDate;
                    currentUserPlant.last_care_date = lastCareDate;
                    currentUserPlant.care_schedule = careSchedule;
                    currentUserPlant.notes = notes;
                }

                // Температура
                var temperature = AppConnect.OrganayzerRasteniyModel.Temperature
                    .FirstOrDefault(t => t.plant_id == plant.id) ?? new Temperature { plant_id = plant.id };

                temperature.min_temp = minTemp.HasValue ? (int?)Convert.ToInt32(minTemp.Value) : null;
                temperature.max_temp = maxTemp.HasValue ? (int?)Convert.ToInt32(maxTemp.Value) : null;

                AppConnect.OrganayzerRasteniyModel.Entry(temperature).State = EntityState.Modified;

                // Полив
                var watering = AppConnect.OrganayzerRasteniyModel.Watering.FirstOrDefault(w => w.plant_id == plant.id) ?? new Watering { plant_id = plant.id };
                watering.amount_ml = wateringAmount;

                AppConnect.OrganayzerRasteniyModel.Entry(watering).State = EntityState.Modified;

                // Подкормка
                var fertilizer = AppConnect.OrganayzerRasteniyModel.Fertilization.FirstOrDefault(f => f.plant_id == plant.id) ?? new Fertilization { plant_id = plant.id };
                fertilizer.fertilizer_id = cbFertilizer.SelectedValue as int?;
                fertilizer.dosage_ml = fertilizerDosage;

                AppConnect.OrganayzerRasteniyModel.Entry(fertilizer).State = EntityState.Modified;

                // Освещение
                var lighting = AppConnect.OrganayzerRasteniyModel.Lighting.FirstOrDefault(l => l.plant_id == plant.id) ?? new Lighting { plant_id = plant.id };
                lighting.level = lightingLevel;
                lighting.uses_lights = usesLights;

                AppConnect.OrganayzerRasteniyModel.Entry(lighting).State = EntityState.Modified;

                // Расположение
                var room = AppConnect.OrganayzerRasteniyModel.Rooms.FirstOrDefault(r => r.name == roomName);

                if (room != null)
                {
                    var location = AppConnect.OrganayzerRasteniyModel.PlantLocations.FirstOrDefault(pl => pl.plant_id == plant.id);

                    if (location == null)
                    {
                        location = new PlantLocations
                        {
                            plant_id = plant.id,
                            room_id = room.id,
                            window_position = windowPosition
                        };

                        AppConnect.OrganayzerRasteniyModel.PlantLocations.Add(location);
                    }
                    else
                    {
                        location.room_id = room.id;
                        location.window_position = windowPosition;
                    }
                }

                // Сохраняем фото
                if (!string.IsNullOrEmpty(_coverImagePath))
                {
                    SavePhoto(plant.id);
                }

                AppConnect.OrganayzerRasteniyModel.SaveChanges();
                MessageBox.Show("Растение успешно сохранено.");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SavePhoto(int plantId)
        {
            if (string.IsNullOrEmpty(_coverImagePath))
                return;

            string fileName = Path.GetFileName(_coverImagePath);
            fileName = fileName.Replace(" ", "_"); // Убираем пробелы

            string targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            string targetPath = Path.Combine(targetDirectory, fileName);

            try
            {
                // Удаляем старые фото для этого растения
                var oldPhotos = AppConnect.OrganayzerRasteniyModel.Photos.Where(p => p.plant_id == plantId).ToList();
                AppConnect.OrganayzerRasteniyModel.Photos.RemoveRange(oldPhotos);

                // Копируем новое изображение
                if (!File.Exists(targetPath))
                    File.Copy(_coverImagePath, targetPath, overwrite: true);

                Photos newPhoto = new Photos
                {
                    plant_id = plantId,
                    photo_path = $"/Images/{fileName}",
                    upload_date = DateTime.Now
                };

                AppConnect.OrganayzerRasteniyModel.Photos.Add(newPhoto);
                AppConnect.OrganayzerRasteniyModel.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}");
            }
        }
    }
}