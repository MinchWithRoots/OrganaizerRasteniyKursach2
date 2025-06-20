using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text.Trim();
            string login = tbLogin.Text.Trim();
            string email = tbEmail.Text.Trim();
            string phone = tbPhone.Text.Trim();
            string password = tbPassword.Password.Trim();
            string repeatPassword = tbRepeatPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            if (!Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
            {
                MessageBox.Show("Некорректный email");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show("Введите корректный номер телефона");
                return;
            }

            if (AppConnect.OrganayzerRasteniyModel.Users.Any(u => u.login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }

            var user = new Users
            {
                name = name,
                login = login,
                email = email,
                phone = phone,
                password = password
            };

            AppConnect.OrganayzerRasteniyModel.Users.Add(user);
            AppConnect.OrganayzerRasteniyModel.SaveChanges();
            MessageBox.Show("Успешная регистрация!");
            AppData.MainFrame.FrameMain.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.GoBack();
        }
    }
}
