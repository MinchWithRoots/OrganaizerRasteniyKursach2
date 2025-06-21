using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage() => InitializeComponent();

        /* регистрация ------------------------------------------------------- */
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text.Trim();
            string login = tbLogin.Text.Trim();
            string email = tbEmail.Text.Trim();
            string phone = tbPhone.Text.Trim();
            string password = tbPassword.Password.Trim();
            string repeatPassword = tbRepeatPassword.Password.Trim();

            var sb = new StringBuilder();

            /* базовые проверки */
            if (string.IsNullOrWhiteSpace(name)) sb.AppendLine("Заполните имя");
            if (string.IsNullOrWhiteSpace(login)) sb.AppendLine("Заполните логин");
            if (string.IsNullOrWhiteSpace(email)) sb.AppendLine("Введите email");
            if (string.IsNullOrWhiteSpace(phone)) sb.AppendLine("Введите телефон");
            if (string.IsNullOrWhiteSpace(password)) sb.AppendLine("Введите пароль");
            if (string.IsNullOrWhiteSpace(repeatPassword)) sb.AppendLine("Повторите пароль");
            if (password != repeatPassword) sb.AppendLine("Пароли не совпадают");

            /* валидации */
            if (!Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                sb.AppendLine("Некорректный email");

            if (!Regex.IsMatch(phone, @"^\+?\d+$"))
                sb.AppendLine("Телефон должен начинаться с '+' и содержать только цифры");

            if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
                sb.AppendLine("Пароль ≥ 8 символов, минимум 1 буква и 1 цифра");

            var ctx = AppConnect.OrganayzerRasteniyModel;
            if (ctx.Users.Any(u => u.login == login))
                sb.AppendLine("Такой логин уже существует");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Ошибка регистрации",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            /* добавление --------------------------------------------------- */
            var user = new Users
            {
                name = name,
                login = login,
                email = email,
                phone = phone,
                password = password
            };

            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно",
                                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                AppData.MainFrame.FrameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message,
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /* назад ------------------------------------------------------------ */
        private void Back_Click(object sender, RoutedEventArgs e) =>
            AppData.MainFrame.FrameMain.GoBack();
    }
}
