﻿using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repository
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public void Update(Product product);
    }
}
