using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace CongDoanCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
    }
}