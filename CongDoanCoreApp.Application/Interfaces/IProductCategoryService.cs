using CongDoanCoreApp.Application.ViewModels.Product;
using System;
using System.Collections.Generic;

namespace CongDoanCoreApp.Application.Interfaces
{
    public interface IProductCategoryService:IDisposable
    {
        void Add(ProductCategoryViewModel productCategoryViewModel);

        void Update(ProductCategoryViewModel productCategoryViewModel);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        void ReOrder(int sourceId, int targetId);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        void Save();
    }
}