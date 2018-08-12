using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;

namespace CongDoanCoreApp.Data.EF.Repositories
{
    public class TagRepository : EFRepository<Tag, string>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }
    }
}