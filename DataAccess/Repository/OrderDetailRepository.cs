using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void Create(OrderDetail orderDetail)
        {
            OrderDetailDAO.Instance.AddNew(orderDetail);
        }

        public void Delete(int id, int proId)
        {
            OrderDetail orderDetail = GetOrderDetail( id, proId);
            OrderDetailDAO.Instance.Remove(orderDetail.OrderId,orderDetail.ProductId);
        }

        public OrderDetail GetOrderDetail(int id, int proId)
        {
            return OrderDetailDAO.Instance.GetOrderDetailByIDAndProductID(id, proId);
        }

        public IEnumerable<OrderDetail> GetOrderDetails() => OrderDetailDAO.Instance.GetOrderDetailList();

        public void Update(OrderDetail ordt)
        {
            OrderDetailDAO.Instance.Update(ordt);
        }
    }
}
