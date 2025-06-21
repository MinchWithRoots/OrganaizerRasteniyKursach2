using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage() => InitializeComponent();

        /* вход -------------------------------------------------------------- */
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string password = tbPassword.Visibility == Visibility.Visible
                                ? tbPassword.Password
                                : tbVisiblePwd.Text;

            var ctx = AppConnect.OrganayzerRasteniyModel;

            /* сначала проверяем админа */
            var admin = ctx.Admins.FirstOrDefault(a =>
                            a.login == login && a.password == password);

            if (admin != null)
            {
                App.CurrentUser = new Users { name = admin.name, login = admin.login };
                App.CurrentRole = "Admin";
                MessageBox.Show($"Добро пожаловать, {admin.name}! (Администратор)");
                MainFrame.FrameMain.Navigate(new AdminPlantsPage());
                return;
            }

            /* обычный пользователь */
            var user = ctx.Users.FirstOrDefault(u =>
                        u.login == login && u.password == password);

            if (user != null)
            {
                App.CurrentUser = user;
                App.CurrentRole = "User";
                MessageBox.Show($"Здравствуйте, {user.name}!");
                MainFrame.FrameMain.Navigate(new UserPlantsPage());
                return;
            }

            /* ничего не нашли */
            MessageBox.Show("Неверный логин или пароль",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /* переход к регистрации -------------------------------------------- */
        private void Register_Click(object sender, RoutedEventArgs e) =>
            MainFrame.FrameMain.Navigate(new RegistrationPage());

        /* показать / скрыть пароль ----------------------------------------- */
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            tbVisiblePwd.Text = tbPassword.Password;
            tbVisiblePwd.Visibility = Visibility.Visible;
            tbPassword.Visibility = Visibility.Collapsed;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPassword.Password = tbVisiblePwd.Text;
            tbVisiblePwd.Visibility = Visibility.Collapsed;
            tbPassword.Visibility = Visibility.Visible;
        }
    }
}
