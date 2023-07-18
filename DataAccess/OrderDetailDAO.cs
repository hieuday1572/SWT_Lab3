using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instancelock = new object();
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instancelock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailList()
        {
            var orders = new List<OrderDetail>();
            try
            {
                using var context = new AssSalesContext();
                orders = context.OrderDetails.Include(p=>p.Order.Member).Include(p=>p.Product).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public IEnumerable<OrderDetail> GetOrderDetailByID(int ID)
        {
            var order = new List<OrderDetail>();
            try
            {
                using var context = new AssSalesContext();
                order = context.OrderDetails.Include(p => p.Order.Member).Include(p => p.Product).Where(p => p.OrderId == ID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public void AddNew(OrderDetail order)
        {
            try
            {
                var _order = GetOrderDetailByIDAndProductID(order.OrderId, order.ProductId);
                if (_order == null)
                {
                    using var context = new AssSalesContext();
                    context.OrderDetails.Add(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order detail is already exist");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(OrderDetail order)
        {
            try
            {
                OrderDetail _order = GetOrderDetailByIDAndProductID(order.OrderId, order.ProductId);
                if (_order != null)
                {
                    using var context = new AssSalesContext();
                    context.OrderDetails.Update(order);
                    context.SaveChanges();
                }
                else { throw new Exception("The order detail does not already exist"); }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void Remove(int ID, int ProductID)
        {
            try
            {
                var order = GetOrderDetailByIDAndProductID( ID, ProductID);
                if (order != null)
                {
                    using var context = new AssSalesContext();
                    context.OrderDetails.Remove(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order detail does not already exist");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public OrderDetail GetOrderDetailByIDAndProductID(int OrderID, int ProductID)
        {
            using var context = new AssSalesContext();
            OrderDetail order = context.OrderDetails.Include(p=>p.Order.Member).Include(p=>p.Product).Where(p => p.ProductId == ProductID).Where(p => p.OrderId == OrderID).SingleOrDefault();
            return order;
        }

        public IEnumerable<OrderDetail> GetOrderDetailByProductID(int ID)
        {
            var order = new List<OrderDetail>();
            try
            {
                using var context = new AssSalesContext();
                order = context.OrderDetails.Where(p => p.ProductId == ID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }
    }
}
