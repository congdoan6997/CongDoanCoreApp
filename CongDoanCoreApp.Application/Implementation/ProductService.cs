using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CongDoanCoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FillAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }
    }
}