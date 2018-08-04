using System;

namespace CongDoanCoreApp.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// save change from db context
        /// </summary>
        void Commit();
    }
}