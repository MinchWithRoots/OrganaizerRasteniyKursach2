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
        private bool _isEditMode;
        private string _coverImagePath;

        /* ―――――――――――――― конструкторы ―――――――――――――― */

        // Для нового растения
        public CreatePlantPage()
        {
            InitializeComponent();

            _currentPlant = new Plants();
            DataContext = _currentPlant;     // сразу привязываем
            LoadCategories();
        }

        // Для редактирования
        public CreatePlantPage(Plants plant) : this()
        {
            _currentPlant = plant;
            _isEditMode = true;
            DataContext = _currentPlant;

            /* заполняем поля вручную, чтобы не отформатировать цену */
            tbName.Text = _currentPlant.name;
            tbDescription.Text = _currentPlant.description;
            cbCategories.SelectedValue = _currentPlant.category_id;
            tbPrice.Text = _currentPlant.price?.ToString("F0") ?? "";

            /* картинка */
            try
            {
                var path = GetPhotoPath(_currentPlant.id);
                if (!string.IsNullOrEmpty(path))
                    imgCover.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка изображения: {ex.Message}");
            }
        }

        /* ―――――――――――――― справочники ―――――――――――――― */

        private void LoadCategories()
        {
            cbCategories.ItemsSource = AppConnect.OrganayzerRasteniyModel.Categories.ToList();
            cbCategories.DisplayMemberPath = "name";
            cbCategories.SelectedValuePath = "id";
        }

        /* ―――――――――――――― работа с изображением ―――――――――――――― */

        private void bImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                _coverImagePath = dlg.FileName;
                imgCover.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_coverImagePath));
            }
        }

        /* ―――――――――――――― сохранение ―――――――――――――― */

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            /* --- простая валидация --- */
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Введите название растения.");
                return;
            }

            // Убираем пробелы/неразрывные пробелы из ввода
            var rawPrice = tbPrice.Text.Replace(" ", "").Replace("\u00A0", "");

            if (!int.TryParse(rawPrice, out int price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (целое число, без копеек).");
                return;
            }

            /* --- заполняем данные --- */
            _currentPlant.name = tbName.Text.Trim();
            _currentPlant.description = tbDescription.Text.Trim();
            _currentPlant.category_id = cbCategories.SelectedValue as int?;
            _currentPlant.price = price;

            try
            {
                var ctx = AppConnect.OrganayzerRasteniyModel;

                if (_isEditMode)
                {
                    var dbPlant = ctx.Plants.Find(_currentPlant.id);
                    if (dbPlant == null)
                    {
                        MessageBox.Show("Запись не найдена.");
                        return;
                    }

                    dbPlant.name = _currentPlant.name;
                    dbPlant.description = _currentPlant.description;
                    dbPlant.category_id = _currentPlant.category_id;
                    dbPlant.price = _currentPlant.price;
                }
                else
                {
                    ctx.Plants.Add(_currentPlant);
                }

                ctx.SaveChanges();      // первым делом — получить id для фото

                if (!string.IsNullOrEmpty(_coverImagePath))
                    SavePhoto(_currentPlant.id);

                MessageBox.Show(_isEditMode ? "Растение обновлено!" : "Растение добавлено!");
                NavigationService.Navigate(new AdminPlantsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        /* ―――――――――――――― фото ―――――――――――――― */

        private void SavePhoto(int plantId)
        {
            if (string.IsNullOrEmpty(_coverImagePath)) return;

            string fileName = Path.GetFileName(_coverImagePath).Replace(" ", "_");
            string imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            if (!Directory.Exists(imgDir))
                Directory.CreateDirectory(imgDir);

            string targetPath = Path.Combine(imgDir, fileName);

            /* перезаписываем без вопросов */
            File.Copy(_coverImagePath, targetPath, true);

            var ctx = AppConnect.OrganayzerRasteniyModel;

            // удаляем старые фото
            ctx.Photos.RemoveRange(ctx.Photos.Where(p => p.plant_id == plantId));

            ctx.Photos.Add(new Photos
            {
                plant_id = plantId,
                photo_path = $"/Images/{fileName}",
                upload_date = DateTime.Now
            });

            ctx.SaveChanges();
        }

        private string GetPhotoPath(int plantId)
        {
            var photo = AppConnect.OrganayzerRasteniyModel.Photos
                                   .Where(p => p.plant_id == plantId)
                                   .OrderByDescending(p => p.upload_date)
                                   .FirstOrDefault();

            if (photo == null) return null;

            string full = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photo.photo_path.TrimStart('/'));
            return File.Exists(full) ? full : null;
        }
    }
}
