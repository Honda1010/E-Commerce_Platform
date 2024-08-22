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
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var Category_by_id = _context.Categories.FirstOrDefault(c=> c.Id == category.Id);
            if (category != null) {
                Category_by_id.Name = category.Name;
                Category_by_id.Description = category.Description;
                Category_by_id.CreatedTime = DateTime.Now;
            }
        }
    }
}
