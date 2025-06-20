using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{

    public partial class RegistrationPage : Page
    {
        private Users _currentUser = new Users();
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTb.Text;
            string login = loginTb.Text;
            string password = passwordTb.Password;
            string repeatPassword = repeatPasswordTb.Password;
            string email = emailTb.Text;
            string phone = phoneTb.Text;

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
                sb.AppendLine("Заполните имя");
            if (string.IsNullOrWhiteSpace(login))
                sb.AppendLine("Заполните логин");
            if (string.IsNullOrWhiteSpace(password))
                sb.AppendLine("Введите пароль");
            if (string.IsNullOrWhiteSpace(repeatPassword))
                sb.AppendLine("Повторите пароль");
            if (string.IsNullOrWhiteSpace(email))
                sb.AppendLine("Введите email");
            if (string.IsNullOrWhiteSpace(phone))
                sb.AppendLine("Введите телефон");
            if (password != repeatPassword)
                sb.AppendLine("Пароли не совпадают");

            if (!IsValidEmail(email))
                sb.AppendLine("Некорректный email");
            if (!IsValidPhone(phone))
                sb.AppendLine("Телефон должен начинаться с '+' и содержать только цифры");
            if (!IsValidPassword(password))
                sb.AppendLine("Пароль должен быть не менее 8 символов и содержать хотя бы одну цифру и одну букву");

            var ctx = AppConnect.OrganayzerRasteniyModel;

            if (ctx.Users.Any(u => u.login == login))
                sb.AppendLine("Такой логин уже существует");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Добавление пользователя
            var user = new Users
            {
                name = name,
                login = login,
                password = password,
                email = email,
                phone = phone
            };

            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AppData.MainFrame.FrameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhone(string phone)
        {
            string pattern = @"^\+?\d+$";
            return Regex.IsMatch(phone, pattern);
        }

        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.GoBack();
        }
    }
}