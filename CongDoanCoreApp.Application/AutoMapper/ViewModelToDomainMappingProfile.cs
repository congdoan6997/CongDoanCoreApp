using AutoMapper;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Data.Entities;

namespace CongDoanCoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>().ConstructUsing(
                c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image,
                c.HomeFlag, c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<ProductViewModel, Product>().ConstructUsing(
                p => new Product(p.Name, p.CategoryId, p.Image, p.Price, p.OriginalPrice, p.PromotionPrice
                , p.Description, p.Content, p.HomeFlag, p.HotFlag, p.Tags, p.Unit, p.Status, p.SeoPageTitle,
                p.SeoAlias, p.SeoKeywords, p.SeoDescription));

            CreateMap<AppUserViewModel, AppUser>().ConstructUsing(
                x => new AppUser(x.Id.Value, x.FullName, x.UserName, x.Email, x.PhoneNumber, x.Avatar, x.Status));

            CreateMap<AppRoleViewModel, AppRole>().ConstructUsing(
                x => new AppRole(x.Name, x.Description));
        }
    }
}