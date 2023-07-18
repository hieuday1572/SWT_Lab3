using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();

        void Update(Order order);

        void Create(Order order);

        void Delete(int id);

        Order GetOrder(int id);
    }
}
