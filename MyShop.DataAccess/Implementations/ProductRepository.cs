using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementations
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var Product_by_id = _context.Products.FirstOrDefault(p=> p.Id == product.Id);
            if (product != null) {
                Product_by_id.Name = product.Name;
                Product_by_id.Description = product.Description;
                Product_by_id.price = product.price;
                Product_by_id.Image = product.Image;
                Product_by_id.CategoryId = product.CategoryId;
            }
        }
    }
}
