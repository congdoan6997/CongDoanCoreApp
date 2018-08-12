using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;

namespace CongDoanCoreApp.Data.EF.Repositories
{
    public class ProductTagRepository : EFRepository<ProductTag, int>, IProductTagRepository
    {
        public ProductTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}