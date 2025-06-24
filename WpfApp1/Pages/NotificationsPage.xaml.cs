using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class NotificationsPage : Page
    {
        public NotificationsPage(int userId)
        {
            InitializeComponent();
            LoadNotifications(userId);
        }

        private void LoadNotifications(int userId)
        {
            // Получаем все растения текущего пользователя
            var userPlants = AppConnect.OrganayzerRasteniyModel.UserPlants
                .Where(up => up.user_id == userId)
                .ToList();

            var notifications = new List<NotificationItem>();

            foreach (var plant in userPlants)
            {
                if (!string.IsNullOrEmpty(plant.notes))
                {
                    // Пропускаем записи с "хорошее состояние"
                    if (plant.notes.ToLower().Contains("хорошее состояние"))
                        continue;

                    var plantDetails = AppConnect.OrganayzerRasteniyModel.Plants.FirstOrDefault(p => p.id == plant.plant_id);

                    if (plantDetails != null)
                    {
                        notifications.Add(new NotificationItem
                        {
                            PlantName = plantDetails.name,
                            NotificationMessage = plant.notes
                        });
                    }
                }
            }

            ListNotifications.ItemsSource = notifications;
        }

        public class NotificationItem
        {
            public string PlantName { get; set; }
            public string NotificationMessage { get; set; }
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserPlantsPage());
        }
    }
}