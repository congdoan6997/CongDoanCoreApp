using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CongDoanCoreApp.Data.EF.Repositories
{
    public class ProductRepository : EFRepository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

    }
}
