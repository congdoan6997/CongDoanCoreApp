using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CongDoanCoreApp.Application.Interfaces
{
   public interface IProductService:IDisposable
    {
        List<ProductViewModel> GetAll();
    }
}
