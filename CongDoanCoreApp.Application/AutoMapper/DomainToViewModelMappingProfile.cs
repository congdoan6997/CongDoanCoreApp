using AutoMapper;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Data.Entities;

namespace CongDoanCoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}