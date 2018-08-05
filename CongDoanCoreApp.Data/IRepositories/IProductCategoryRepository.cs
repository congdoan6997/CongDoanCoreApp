using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CongDoanCoreApp.Data.IRepositories
{
    public interface IProductCategoryRepository:IRepository<ProductCategory,int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
