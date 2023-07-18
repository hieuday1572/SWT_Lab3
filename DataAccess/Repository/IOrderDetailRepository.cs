using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetails();

        void Update(OrderDetail ordt);

        void Create(OrderDetail orderDetail);

        void Delete(int id, int proId);

        OrderDetail GetOrderDetail(int id, int proId);
    }
}
