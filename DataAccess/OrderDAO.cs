using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instancelock = new object();
        public static OrderDAO Instance
        {
            get
            {
                lock (instancelock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Order> GetOrderList()
        {
            var orders = new List<Order>();
            try
            {
                using var context = new AssSalesContext();
                orders = context.Orders.Include(p=>p.Member).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public Order GetOrderByID(int ID)
        {
            Order order = null;
            try
            {
                using var context = new AssSalesContext();
                order = context.Orders.Include(p => p.Member).SingleOrDefault(c => c.OrderId == ID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public void AddNew(Order order)
        {
            try
            {
                Order _order = GetOrderByID(order.OrderId);
                if (_order == null)
                {
                    using var context = new AssSalesContext();
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order is already exist");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Order order)
        {
            try
            {
                Order _order = GetOrderByID(order.OrderId);
                if (_order != null)
                {
                    using var context = new AssSalesContext();
                    context.Orders.Update(order);
                    context.SaveChanges();
                }
                else { throw new Exception("The order does not already exist"); }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void Remove(int ID)
        {
            try
            {
                Order order = GetOrderByID(ID);
                if (order != null)
                {
                    using var context = new AssSalesContext();
                    var od = OrderDetailDAO.Instance.GetOrderDetailByID(ID);
                    context.OrderDetails.RemoveRange(od);
                    context.Orders.Remove(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order does not already exist");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}

