using CongDoanCoreApp.Infrastructure.Interfaces;

namespace CongDoanCoreApp.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public EFUnitOfWork(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Commit()
        {
            this._dbContext.SaveChanges();
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}