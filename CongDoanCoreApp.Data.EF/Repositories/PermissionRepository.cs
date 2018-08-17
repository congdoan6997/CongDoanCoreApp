using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;

namespace CongDoanCoreApp.Data.EF.Repositories
{
    public class PermissionRepository : EFRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}