using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class OrdersPage : Page
    {
        private List<OrderViewModel> _allOrders;

        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = AppConnect.OrganayzerRasteniyModel.Orders.ToList();

                _allOrders = new List<OrderViewModel>();

                foreach (var order in orders)
                {
                    var details = AppConnect.OrganayzerRasteniyModel.OrderDetails
                        .Where(od => od.order_id == order.id)
                        .Join(AppConnect.OrganayzerRasteniyModel.Plants,
                              od => od.plant_id,
                              p => p.id,
                              (od, p) => new OrderDetailViewModel
                              {
                                  PlantName = p.name,
                                  Quantity = od.quantity,
                                  Price = od.price,
                                  Total = (od.quantity) * (od.price)
                              })
                        .ToList();

                    _allOrders.Add(new OrderViewModel
                    {
                        OrderID = order.id,
                        UserEmail = order.Users?.email ?? "Не указан",
                        OrderDate = order.order_date,
                        Status = order.status,
                        TotalAmount = order.total_amount,
                        Details = details
                    });
                }

                ApplyFiltersAndSort();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
            }
        }

        private void ApplyFiltersAndSort()
        {
            if (_allOrders == null) return;

            var filteredOrders = _allOrders.ToList();

            // Фильтрация по статусу
            var selectedStatusItem = ComboStatusFilter.SelectedItem as ComboBoxItem;
            string selectedStatus = selectedStatusItem?.Content?.ToString();
            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Все статусы")
            {
                filteredOrders = filteredOrders.Where(o => o.Status == selectedStatus).ToList();
            }

            // Фильтрация по поиску
            string search = (TextSearch?.Text)?.ToLower() ?? string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                filteredOrders = filteredOrders
                    .Where(o => o.OrderID.ToString().Contains(search) ||
                               o.UserEmail.ToLower().Contains(search))
                    .ToList();
            }

            // Сортировка
            var comboSort = ComboSort.SelectedItem as ComboBoxItem;
            if (comboSort != null)
            {
                string sortOption = comboSort.Content.ToString();
                switch (sortOption)
                {
                    case "Дата: по возрастанию":
                        filteredOrders = filteredOrders.OrderBy(o => o.OrderDate).ToList();
                        break;
                    case "Дата: по убыванию":
                        filteredOrders = filteredOrders.OrderByDescending(o => o.OrderDate).ToList();
                        break;
                    case "Сумма: по возрастанию":
                        filteredOrders = filteredOrders.OrderBy(o => o.TotalAmount).ToList();
                        break;
                    case "Сумма: по убыванию":
                        filteredOrders = filteredOrders.OrderByDescending(o => o.TotalAmount).ToList();
                        break;
                }
            }

            // Обновление отображения
            OrdersList.ItemsSource = filteredOrders;
        }

        private void ComboStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void ComboSort_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltersAndSort();
        }

        public class OrderViewModel
        {
            public int OrderID { get; set; }
            public string UserEmail { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal TotalAmount { get; set; }
            public List<OrderDetailViewModel> Details { get; set; }
        }

        public class OrderDetailViewModel
        {
            public string PlantName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }
    }
}