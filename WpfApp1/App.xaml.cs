using System;
using System.Linq;
using System.Windows;
using WpfApp1.AppData;

namespace WpfApp1
{
    public partial class App : Application
    {
        public static Users CurrentUser { get; set; }
        public static string CurrentRole { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Проверяем, инициализирован ли контекст
            if (AppConnect.OrganayzerRasteniyModel == null)
            {
                MessageBox.Show("Ошибка подключения к базе данных.");
                Shutdown();
                return;
            }

            // Можно добавить тестовое чтение данных для проверки подключения
            try
            {
                var test = AppConnect.OrganayzerRasteniyModel.Categories.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}");
                Shutdown();
            }
        }
    }
}