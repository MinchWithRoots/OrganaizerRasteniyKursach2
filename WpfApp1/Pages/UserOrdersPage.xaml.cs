using System;
using System.Collections.Generic;
using System.Linq;
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

        /* View-модели */
        private class OrderVm
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal Total { get; set; }
            public List<ItemVm> Items { get; set; }
        }
        private class ItemVm
        {
            public string Name { get; set; }
            public int Qty { get; set; }
            public decimal Sum { get; set; }
        }

        private void LoadOrders()
        {
            int userId = App.CurrentUser.id;
            var ctx = AppConnect.OrganayzerRasteniyModel;

            /* Основные заказы */
            var orders = ctx.Orders
                            .Where(o => o.user_id == userId)
                            .OrderByDescending(o => o.order_date)
                            .ToList();

            /* Детали + товары, чтобы не делать много запросов */
            var orderIds = orders.Select(o => o.id).ToList();
            var details = ctx.OrderDetails
                              .Where(d => orderIds.Contains(d.order_id))
                              .ToList();

            var plants = ctx.Plants.ToList();

            var list = new List<OrderVm>();

            foreach (var o in orders)
            {
                var od = details.Where(d => d.order_id == o.id).ToList();

                var items = od.Select(d =>
                {
                    var plant = plants.FirstOrDefault(p => p.id == d.plant_id);
                    return new ItemVm
                    {
                        Name = plant != null ? plant.name : $"ID {d.plant_id}",
                        Qty = d.quantity,
                        Sum = d.price * d.quantity
                    };
                }).ToList();

                list.Add(new OrderVm
                {
                    OrderId = o.id,
                    OrderDate = o.order_date,
                    Status = o.status,
                    Total = items.Sum(i => i.Sum),
                    Items = items
                });
            }

            ListOrders.ItemsSource = list;
        }
    }
}

