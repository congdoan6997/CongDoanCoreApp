using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Data.IRepositories;
using CongDoanCoreApp.Utilities.Dtos;
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

        public PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FillAll(x => x.Status == Data.Enums.Status.Active);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize);
            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paginationSet = new PageResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = totalRow
            };
            return paginationSet;
        }
    }
}