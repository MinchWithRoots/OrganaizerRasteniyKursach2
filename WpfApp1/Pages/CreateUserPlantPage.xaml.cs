using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class CreateUserPlantPage : Page
    {
        /* ────────────────── поля ────────────────── */
        private PlantViewItem _plantItem;
        private bool _isNew = true;
        private string _coverImagePath;          // может остаться null

        /* ────────────────── ctor ────────────────── */
        public CreateUserPlantPage() : this(null) { }

        public CreateUserPlantPage(PlantViewItem plantItem)
        {
            InitializeComponent();

            /* начальное состояние */
            _coverImagePath = null;

            if (plantItem != null)
            {
                _plantItem = plantItem;
                _isNew = false;

                /* заполняем форму */
                txtName.Text = _plantItem.Name;
                txtDescription.Text = _plantItem.Description;
                dpPurchaseDate.SelectedDate = _plantItem.PurchaseDate;
                dpLastCareDate.SelectedDate = _plantItem.LastCareDate;
                txtCareSchedule.Text = _plantItem.CareSchedule;
                txtNotes.Text = _plantItem.Notes;
                txtMinTemp.Text = _plantItem.MinTemp?.ToString() ?? "";
                txtMaxTemp.Text = _plantItem.MaxTemp?.ToString() ?? "";
                txtWateringAmount.Text = _plantItem.WateringAmount?.ToString() ?? "";
                txtFertilizerDosage.Text = _plantItem.FertilizerDosage?.ToString() ?? "";
                cbLightingLevel.SelectedItem = _plantItem.LightingLevel;
                cbUsesLights.IsChecked = _plantItem.UsesLights;
                txtWindowPosition.Text = _plantItem.WindowPosition;

                if (!string.IsNullOrWhiteSpace(_plantItem.PhotoPath))
                    imgPreview.Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri(_plantItem.PhotoPath, UriKind.Absolute));
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

        /* ────────────────── справочники ────────────────── */
        private void LoadDefaultData()
        {
            cbCategory.ItemsSource = AppConnect.OrganayzerRasteniyModel.Categories.Select(c => c.name).ToList();
            cbFertilizer.ItemsSource = AppConnect.OrganayzerRasteniyModel.Fertilizers.Select(f => f.name).ToList();
            cbRoom.ItemsSource = AppConnect.OrganayzerRasteniyModel.Rooms.Select(r => r.name).ToList();

            if (!_isNew)
            {
                cbCategory.SelectedItem = _plantItem.CategoryName;
                cbFertilizer.SelectedItem = _plantItem.FertilizerName;
                cbRoom.SelectedItem = _plantItem.RoomName;
            }
        }

        /* ────────────────── выбор фото ────────────────── */
        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png" };
            if (dlg.ShowDialog() == true)
            {
                _coverImagePath = dlg.FileName;
                imgPreview.Source = new System.Windows.Media.Imaging.BitmapImage(
                                        new Uri(_coverImagePath, UriKind.Absolute));
            }
        }

        /* ────────────────── сохранение ────────────────── */
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /* ---------- читаем форму ---------- */
                string name = txtName.Text.Trim();
                string description = txtDescription.Text.Trim();
                int? categoryId = cbCategory.SelectedValue as int?;
                DateTime purchaseDt = dpPurchaseDate.SelectedDate ?? DateTime.Now;
                DateTime? lastCare = dpLastCareDate.SelectedDate;
                string careSchedule = txtCareSchedule.Text.Trim();
                string notes = txtNotes.Text.Trim();

                /* C# 7.3: без условного выражения с null */
                decimal? minTemp = null;
                decimal tVal;
                if (decimal.TryParse(txtMinTemp.Text, out tVal)) minTemp = tVal;

                decimal? maxTemp = null;
                if (decimal.TryParse(txtMaxTemp.Text, out tVal)) maxTemp = tVal;

                int? wateringAmount = null;
                int iVal;
                if (int.TryParse(txtWateringAmount.Text, out iVal)) wateringAmount = iVal;

                int? fertilizerDose = null;
                if (int.TryParse(txtFertilizerDosage.Text, out iVal)) fertilizerDose = iVal;

                string lightingLvl = cbLightingLevel.SelectedItem != null
                                     ? cbLightingLevel.SelectedItem.ToString()
                                     : null;
                bool? usesLights = cbUsesLights.IsChecked;

                string roomName = cbRoom.SelectedItem != null ? cbRoom.SelectedItem.ToString() : null;
                string windowPos = txtWindowPosition.Text.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Введите название растения.");
                    return;
                }

                var ctx = AppConnect.OrganayzerRasteniyModel;

                /* ---------- 1. PLANTS ---------- */
                Plants plant;
                if (_isNew)
                {
                    plant = new Plants
                    {
                        name = name,
                        description = description,
                        category_id = categoryId
                    };
                    ctx.Plants.Add(plant);
                    ctx.SaveChanges();            // нужен id
                    _plantItem.Id = plant.id;
                    _isNew = false;
                }
                else
                {
                    plant = ctx.Plants.Find(_plantItem.Id);
                    if (plant == null)
                    {
                        MessageBox.Show("Растение не найдено.");
                        return;
                    }
                    plant.name = name;
                    plant.description = description;
                    plant.category_id = categoryId;
                    ctx.SaveChanges();
                }

                /* ---------- 2. USER_PLANTS ---------- */
                var up = ctx.UserPlants
                            .FirstOrDefault(x => x.user_id == App.CurrentUser.id && x.plant_id == plant.id);

                if (up == null)
                {
                    up = new UserPlants
                    {
                        user_id = App.CurrentUser.id,
                        plant_id = plant.id,
                        purchase_date = purchaseDt
                    };
                    ctx.UserPlants.Add(up);
                }

                up.purchase_date = purchaseDt;
                up.last_care_date = lastCare;
                up.care_schedule = careSchedule;
                up.notes = notes;

                /* ---------- 3. Upsert-helpers ---------- */
                UpsertTemperature(ctx, plant.id, minTemp, maxTemp);
                UpsertWatering(ctx, plant.id, wateringAmount);
                UpsertFertilizer(ctx, plant.id, fertilizerDose);
                UpsertLighting(ctx, plant.id, lightingLvl, usesLights);
                UpsertLocation(ctx, plant.id, roomName, windowPos);

                ctx.SaveChanges();   // основные данные

                /* ---------- 4. Фото ---------- */
                if (!string.IsNullOrEmpty(_coverImagePath))
                    SavePhoto(ctx, plant.id);

                MessageBox.Show("Растение успешно сохранено.");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        /* ────────────────── Upsert-helpers ────────────────── */
        private static void UpsertTemperature(OrganayzerRasteniyEntities1 ctx, int plantId,
                                              decimal? min, decimal? max)
        {
            var t = ctx.Temperature.FirstOrDefault(x => x.plant_id == plantId);
            if (t == null)
            {
                t = new Temperature { plant_id = plantId };
                ctx.Temperature.Add(t);
            }
            t.min_temp = min.HasValue ? (int?)Convert.ToInt32(min.Value) : null;
            t.max_temp = max.HasValue ? (int?)Convert.ToInt32(max.Value) : null;
        }

        private static void UpsertWatering(OrganayzerRasteniyEntities1 ctx, int plantId, int? amount)
        {
            var w = ctx.Watering.FirstOrDefault(x => x.plant_id == plantId);
            if (w == null)
            {
                w = new Watering { plant_id = plantId };
                ctx.Watering.Add(w);
            }
            w.amount_ml = amount;
        }

        private void UpsertFertilizer(OrganayzerRasteniyEntities1 ctx, int plantId, int? dosage)
        {
            var f = ctx.Fertilization.FirstOrDefault(x => x.plant_id == plantId);
            if (f == null)
            {
                f = new Fertilization { plant_id = plantId };
                ctx.Fertilization.Add(f);
            }
            f.fertilizer_id = cbFertilizer.SelectedValue as int?;
            f.dosage_ml = dosage;
        }

        private static void UpsertLighting(OrganayzerRasteniyEntities1 ctx, int plantId,
                                           string level, bool? usesLights)
        {
            var l = ctx.Lighting.FirstOrDefault(x => x.plant_id == plantId);
            if (l == null)
            {
                l = new Lighting { plant_id = plantId };
                ctx.Lighting.Add(l);
            }
            l.level = level;
            l.uses_lights = usesLights;
        }

        private static void UpsertLocation(OrganayzerRasteniyEntities1 ctx, int plantId,
                                           string roomName, string windowPos)
        {
            if (string.IsNullOrWhiteSpace(roomName)) return;

            var room = ctx.Rooms.FirstOrDefault(r => r.name == roomName);
            if (room == null) return;

            var loc = ctx.PlantLocations.FirstOrDefault(x => x.plant_id == plantId);
            if (loc == null)
            {
                loc = new PlantLocations { plant_id = plantId };
                ctx.PlantLocations.Add(loc);
            }
            loc.room_id = room.id;
            loc.window_position = windowPos;
        }

        /* ────────────────── Фото ────────────────── */
        private void SavePhoto(OrganayzerRasteniyEntities1 ctx, int plantId)
        {
            if (string.IsNullOrEmpty(_coverImagePath)) return;

            string fileName = Path.GetFileName(_coverImagePath).Replace(" ", "_");
            string imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            Directory.CreateDirectory(imgDir);
            string target = Path.Combine(imgDir, fileName);

            if (!File.Exists(target))
                File.Copy(_coverImagePath, target, true);

            var photo = ctx.Photos.FirstOrDefault(p => p.plant_id == plantId);
            if (photo == null)
            {
                photo = new Photos { plant_id = plantId };
                ctx.Photos.Add(photo);
            }
            photo.photo_path = "/Images/" + fileName;
            photo.upload_date = DateTime.Now;

            ctx.SaveChanges();          // только фото
        }

        /* ────────────────── Cancel ────────────────── */
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}