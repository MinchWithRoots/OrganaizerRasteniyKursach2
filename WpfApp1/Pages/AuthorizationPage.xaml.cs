using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Password;

            // Проверяем наличие пользователя в таблице Admins
            var admin = AppConnect.OrganayzerRasteniyModel.Admins.FirstOrDefault(a => a.login == login && a.password == password);

            if (admin != null)
            {
                // Это администратор
                App.CurrentUser = new Users { name = admin.name, login = admin.login }; // Создаем "фиктивного" пользователя
                App.CurrentRole = "Admin";
                MessageBox.Show($"Добро пожаловать, {admin.name}! (Администратор)");
                MainFrame.FrameMain.Navigate(new AdminPlantsPage());
                return;
            }

            // Если администратора нет, проверяем таблицу Users
            var user = AppConnect.OrganayzerRasteniyModel.Users.FirstOrDefault(u => u.login == login && u.password == password);

            if (user != null)
            {
                // Это обычный пользователь
                App.CurrentUser = user;
                App.CurrentRole = "User";
                MessageBox.Show($"Здравствуйте, {user.name}!");
                MainFrame.FrameMain.Navigate(new UserPlantsPage());
                return;
            }

            // Если ни администратор, ни пользователь не найдены
            MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.FrameMain.Navigate(new RegistrationPage());
        }
    }
}