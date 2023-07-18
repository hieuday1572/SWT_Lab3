using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void Create(Order order)
        {
            OrderDAO.Instance.AddNew(order);
        }

        public void Delete(int id)
        {
            Order order = GetOrder(id);
            OrderDAO.Instance.Remove(order.OrderId);
        }

        public Order GetOrder(int id)
        {
            return OrderDAO.Instance.GetOrderByID(id);
        }

        public IEnumerable<Order> GetOrders() => OrderDAO.Instance.GetOrderList();

        public void Update(Order order)
        {
            OrderDAO.Instance.Update(order);
        }
    }
}
