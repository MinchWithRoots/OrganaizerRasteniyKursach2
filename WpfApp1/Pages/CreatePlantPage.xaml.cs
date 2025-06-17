using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class CreatePlantPage : Page
    {
        private Plants _currentPlant;
        private bool _isEditMode = false;
        private string _coverImagePath = null;

        // Конструктор для создания нового растения
        public CreatePlantPage()
        {
            InitializeComponent();

            _currentPlant = new Plants();
            DataContext = this;
            LoadCategories();
        }

        public CreatePlantPage(Plants plant) : this()
        {
            _currentPlant = plant;
            _isEditMode = true;
            DataContext = _currentPlant;

            // Устанавливаем значения на форму
            tbName.Text = _currentPlant.name;
            tbDescription.Text = _currentPlant.description;
            dpPlantingDate.SelectedDate = _currentPlant.planting_date;
            cbCategories.SelectedValue = _currentPlant.category_id;
            tbPrice.Text = _currentPlant.price?.ToString("0.0") ?? "";

            // Загружаем изображение
            try
            {
                string imagePath = GetPhotoPath(_currentPlant.id);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    imgCover.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
            }
        }

        private void LoadCategories()
        {
            cbCategories.ItemsSource = AppConnect.OrganayzerRasteniyModel.Categories.ToList();
            cbCategories.DisplayMemberPath = "name";
            cbCategories.SelectedValuePath = "id";
        }

        private void bImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                _coverImagePath = openFileDialog.FileName;
                imgCover.Source = new System.Windows.Media.Imaging.BitmapImage(
                    new Uri(_coverImagePath, UriKind.Absolute));
            }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Введите название растения.");
                return;
            }

            if (!decimal.TryParse(tbPrice.Text.Replace('.', ','), out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            // Заполняем данные из полей
            _currentPlant.name = tbName.Text.Trim();
            _currentPlant.description = tbDescription.Text.Trim();
            _currentPlant.planting_date = dpPlantingDate.SelectedDate;
            _currentPlant.category_id = cbCategories.SelectedValue as int?;
            _currentPlant.price = price;

            try
            {
                if (_isEditMode)
                {
                    var dbPlant = AppConnect.OrganayzerRasteniyModel.Plants.Find(_currentPlant.id);
                    if (dbPlant != null)
                    {
                        // Обновляем поля
                        dbPlant.name = _currentPlant.name;
                        dbPlant.description = _currentPlant.description;
                        dbPlant.planting_date = _currentPlant.planting_date;
                        dbPlant.category_id = _currentPlant.category_id;
                        dbPlant.price = _currentPlant.price;

                        AppConnect.OrganayzerRasteniyModel.SaveChanges();

                        if (!string.IsNullOrEmpty(_coverImagePath))
                        {
                            SavePhoto(_currentPlant.id);
                            // Обновляем путь к изображению в текущем объекте
                           
                        }
                        // Перезагружаем страницу с принудительной перезагрузкой фото
                        NavigationService.Navigate(new AdminPlantsPage());
                    }
                }

                else
                {
                    // Добавляем новое растение
                    AppConnect.OrganayzerRasteniyModel.Plants.Add(_currentPlant);
                    AppConnect.OrganayzerRasteniyModel.SaveChanges();

                    // Сохраняем фото, если выбрано
                    if (!string.IsNullOrEmpty(_coverImagePath))
                    {
                        SavePhoto(_currentPlant.id);
                    }
                }

                MessageBox.Show(_isEditMode ? "Растение обновлено!" : "Растение добавлено!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void SavePhoto(int plantId)
        {
            if (string.IsNullOrEmpty(_coverImagePath)) return;

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
                    File.Copy(_coverImagePath, targetPath, true);

                Photos newPhoto = new Photos
                {
                    plant_id = plantId,
                    photo_path = $"/Images/{fileName}", // Относительный путь
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

        private string GetPhotoPath(int plantId)
        {
            var photo = AppConnect.OrganayzerRasteniyModel.Photos
                .Where(p => p.plant_id == plantId)
                .OrderByDescending(p => p.upload_date)
                .FirstOrDefault();

            if (photo != null && !string.IsNullOrEmpty(photo.photo_path))
            {
                // Проверяем существование файла на диске
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photo.photo_path.TrimStart('/'));
                if (File.Exists(fullPath))
                    return fullPath;
            }

            // Дефолтное изображение через pack URI
            return "pack://application:,,,/Images/no-image.png";
        }

    }
}