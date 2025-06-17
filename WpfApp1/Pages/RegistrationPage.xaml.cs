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
            DateTime? birthDate = birthDateTb.SelectedDate;
            string experience = experienceTb.Text;
            string phone = phoneTb.Text;

            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(name))
            {
                stringBuilder.AppendLine("Заполните имя\n");
            }
            if (string.IsNullOrWhiteSpace(login))
            {
                stringBuilder.AppendLine("Заполните логин\n");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                stringBuilder.AppendLine("Пароль не должен быть пустым\n");
            }
            if (string.IsNullOrWhiteSpace(repeatPassword))
            {
                stringBuilder.AppendLine("Повторите пароль\n");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                stringBuilder.AppendLine("Заполните email\n");
            }
            if (string.IsNullOrWhiteSpace(experience))
            {
                stringBuilder.AppendLine("Заполните стаж\n");
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                stringBuilder.AppendLine("Заполните телефон\n");
            }
            if (password != repeatPassword)
            {
                stringBuilder.AppendLine("Пароли не совпадают\n");
            }
            if (birthDate.HasValue && birthDate.Value > DateTime.Today)
            {
                stringBuilder.AppendLine("Дата рождения не может быть в будущем\n");
            }
            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString());
                return;
            }

            // Проверка на уникальность логина
            if (AppData.AppConnect.OrganayzerRasteniyModel.Users.Any(u => u.email == login))
            {
                MessageBox.Show("Такой логин уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валидация email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Некорректный email. Попробуйте снова", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валидация телефона
            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Некорректный телефон. Телефон может содержать только символ + и цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валидация пароля
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Некорректный пароль. Пароль должен быть не менее 8 символов и содержать как минимум одну цифру и одну букву", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание нового пользователя
            _currentUser.name = name;
            _currentUser.email = login;
            _currentUser.password = password;
            _currentUser.email = email;
            //_currentUser.BirthDate = birthDate;
            //_currentUser.Experience = experience;
            _currentUser.phone = phone;

            try
            {
                AppData.AppConnect.OrganayzerRasteniyModel.Users.Add(_currentUser);
                AppData.AppConnect.OrganayzerRasteniyModel.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AppData.MainFrame.FrameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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