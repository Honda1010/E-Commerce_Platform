using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repository
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        public void Update(Category category);
    }
}
