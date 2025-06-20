using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp1.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Visibility == Visibility.Visible ? tbPassword.Password : tbVisiblePwd.Text;

            MessageBox.Show($"Логин: {login}\nПароль: {password}", "Вход");
            // Здесь логика авторизации
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegistrationPage());
        }

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
