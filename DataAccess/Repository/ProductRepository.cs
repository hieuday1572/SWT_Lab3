using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void Create(Product product)
        {
            ProductDAO.Instance.AddNew(product);
        }

        public void Delete(int id)
        {
            Product product = GetProduct(id);
            ProductDAO.Instance.Remove(product.ProductId);
        }

        public Product GetProduct(int id)
        {
            return ProductDAO.Instance.GetProductByID(id);
        }

        public IEnumerable<Product> GetProducts() => ProductDAO.Instance.GetProductList();

        public void Update(Product product)
        {
            ProductDAO.Instance.Update(product);
        }
    }
}
