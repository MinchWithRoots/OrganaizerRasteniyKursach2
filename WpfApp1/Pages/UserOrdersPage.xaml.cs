using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class UserOrdersPage : Page
    {
        public UserOrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        /* ────── view-модели ────── */
        private class OrderVm
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal TotalAmount { get; set; }   // из Orders.total_amount
            public List<ItemVm> Items { get; set; }
        }

        private class ItemVm
        {
            public string Name { get; set; }
            public int Qty { get; set; }
            public decimal Sum { get; set; }
        }

        /* ────── загрузка заказов ────── */
        private void LoadOrders()
        {
            int uid = App.CurrentUser.id;
            var ctx = AppConnect.OrganayzerRasteniyModel;

            /* берём заказы пользователя */
            var orders = ctx.Orders
                            .Where(o => o.user_id == uid)
                            .OrderByDescending(o => o.order_date)
                            .ToList();

            if (orders.Count == 0)
            {
                ListOrders.ItemsSource = null;
                return;
            }

            /* предварительно подгружаем детали и растения разом */
            var orderIds = orders.Select(o => o.id).ToList();
            var details = ctx.OrderDetails
                              .Where(d => orderIds.Contains(d.order_id))
                              .ToList();
            var plants = ctx.Plants.ToList();

            var list = new List<OrderVm>();

            foreach (var o in orders)
            {
                var det = details.Where(d => d.order_id == o.id).ToList();

                var items = det.Select(d =>
                {
                    var plant = plants.FirstOrDefault(p => p.id == d.plant_id);
                    decimal pr = d.price != 0 ? d.price : (plant?.price ?? 0m);
                    return new ItemVm
                    {
                        Name = plant != null ? plant.name : $"ID {d.plant_id}",
                        Qty = d.quantity,
                        Sum = pr * d.quantity
                    };
                }).ToList();

                list.Add(new OrderVm
                {
                    OrderId = o.id,
                    OrderDate = o.order_date,
                    Status = o.status,
                    TotalAmount = o.total_amount,     // из таблицы Orders
                    Items = items
                });
            }

            ListOrders.ItemsSource = list;
        }

        private void BackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserPlantsPage());
        }

    }
}
