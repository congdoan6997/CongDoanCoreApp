using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace CongDoanCoreApp.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}