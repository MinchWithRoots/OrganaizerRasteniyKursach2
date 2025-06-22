using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class EditDiscountPage : Page
    {
        private int? _discountId = null;
        private Discounts _currentDiscount = new Discounts();

        public EditDiscountPage(int discountId = -1)
        {
            InitializeComponent();

            LoadPlants();

            if (discountId > 0)
            {
                _discountId = discountId;
                LoadDiscount(discountId);
            }
        }

        private void LoadPlants()
        {
            cbPlants.ItemsSource = AppConnect.OrganayzerRasteniyModel.Plants.ToList();
        }

        private void LoadDiscount(int discountId)
        {
            var discount = AppConnect.OrganayzerRasteniyModel.Discounts.FirstOrDefault(d => d.id == discountId);
            if (discount != null)
            {
                _currentDiscount = discount;
                tbDiscountPercent.Text = discount.discount_percent.ToString();
                dpStart.SelectedDate = discount.start_date;
                dpEnd.SelectedDate = discount.end_date;
              
                tbDescription.Text = discount.description;
                cbPlants.SelectedValue = discount.plant_id;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(tbDiscountPercent.Text, out decimal discountPercent))
            {
                MessageBox.Show("Введите корректный процент скидки.");
                return;
            }

            if (cbPlants.SelectedItem is Plants selectedPlant)
            {
                if (_discountId.HasValue && _discountId > 0)
                {
                    // Редактирование
                    _currentDiscount.plant_id = selectedPlant.id;
                    _currentDiscount.discount_percent = discountPercent;
                    _currentDiscount.start_date = dpStart.SelectedDate ?? DateTime.Now;
                    _currentDiscount.end_date = dpEnd.SelectedDate ?? DateTime.Now;
                   
                    _currentDiscount.description = tbDescription.Text;

                    AppConnect.OrganayzerRasteniyModel.SaveChanges();
                }
                else
                {
                    // Добавление новой скидки
                    var newDiscount = new Discounts
                    {
                        plant_id = selectedPlant.id,
                        discount_percent = discountPercent,
                        start_date = dpStart.SelectedDate ?? DateTime.Now,
                        end_date = dpEnd.SelectedDate ?? DateTime.Now,
                       
                        description = tbDescription.Text
                    };
                    AppConnect.OrganayzerRasteniyModel.Discounts.Add(newDiscount);
                    AppConnect.OrganayzerRasteniyModel.SaveChanges();
                }

                NavigationService.Navigate(new DiscountsPage());
            }
            else
            {
                MessageBox.Show("Выберите растение");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}