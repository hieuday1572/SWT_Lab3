using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instancelock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instancelock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetProductList()
        {
            var products = new List<Product>();
            try
            {
                using var context = new AssSalesContext();
                products = context.Products.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public Product GetProductByID(int ID)
        {
            Product product = null;
            try
            {
                using var context = new AssSalesContext();
                product = context.Products.SingleOrDefault(c => c.ProductId == ID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void AddNew(Product product)
        {
            try
            {
                Product _produuct = GetProductByID(product.ProductId);
                if (_produuct == null)
                {
                    using var context = new AssSalesContext();
                    context.Products.Add(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product is already exist");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Product product)
        {
            try
            {
                Product _product = GetProductByID(product.ProductId);
                if (_product != null)
                {
                    using var context = new AssSalesContext();
                    context.Products.Update(product);
                    context.SaveChanges();
                }
                else { throw new Exception("The product does not already exist"); }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void Remove(int ID)
        {
            try
            {
                Product product = GetProductByID(ID);
                if (product != null)
                {
                    using var context = new AssSalesContext();
                    var od = OrderDetailDAO.Instance.GetOrderDetailByProductID(product.ProductId);
                    context.OrderDetails.RemoveRange(od);
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product does not already exist");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public Product Search(int id, string name, double price, int stock)
        {
            Product product = null;
            var connectionString = @"server =(local); database = FStore;uid=sa;pwd=123456;";
            string sql = "SELECT * FROM Product" +
                        "where 1=1";
            if (id != null)
            {
                sql += "and ProductId = " + id;
            }
            if (name != null)
            {
                sql += "and ProductName like '%" + name + "%'";
            }
            if (price != null)
            {
                sql += "and UnitPrice <= '" + price + "'";
            }
            if (stock != null)
            {
                sql += "and UnitslnStock = " + stock;
            }
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                var sqlDataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        product.ProductId = sqlDataReader.GetInt32(0);
                        product.CategoryId = sqlDataReader.GetInt32(1);
                        product.ProductName = sqlDataReader.GetString(2);
                        product.Weight = sqlDataReader.GetString(3);
                        product.UnitPrice = (decimal)sqlDataReader.GetSqlMoney(4);
                        product.UnitsInStock = sqlDataReader.GetInt32(5);
                    }
                }
            }
            return product;
        }
    }
}
