using AutoMapper;
using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;
using CongDoanCoreApp.Infrastructure.Interfaces;
using CongDoanCoreApp.Utilities.Constants;
using CongDoanCoreApp.Utilities.Dtos;
using CongDoanCoreApp.Utilities.Helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CongDoanCoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, ITagRepository tagRepository, IProductTagRepository productTagRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productViewModel)
        {
            ICollection<ProductTag> producttags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                string[] tags = productViewModel.Tags.Split(',');
                foreach (string item in tags)
                {
                    var tagId = TextHelper.ToUnsignString(item);
                    if (!_tagRepository.FillAll(x => x.Id == tagId).Any())
                    {
                        _tagRepository.Add(new Tag()
                        {
                            Name = item,
                            Id = tagId,
                            Type = CommonConstants.ProductTag
                        });
                    }
                    producttags.Add(new ProductTag()
                    {
                        TagId = tagId
                    });
                }
                var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
                foreach (var item in producttags)
                {
                    product.ProductTags.Add(item);
                }
                _productRepository.Add(product);
            }

            return productViewModel;
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FillAll(x => x.ProductCategory).OrderBy(x => x.DateModified).ProjectTo<ProductViewModel>().ToList();
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

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    decimal.TryParse(worksheet.Cells[i, 3].Value.ToString(), out var originalPrice);

                    decimal.TryParse(worksheet.Cells[i, 4].Value.ToString(), out var price);

                    decimal.TryParse(worksheet.Cells[i, 5].Value.ToString(), out var promotionPrice);

                    bool.TryParse(worksheet.Cells[i, 9].Value.ToString(), out var hotFlag);

                    bool.TryParse(worksheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product = new Product()
                    {
                        CategoryId = categoryId,
                        Name = worksheet.Cells[i, 1].Value.ToString(),
                        Description = worksheet.Cells[i, 2].Value.ToString(),
                        OriginalPrice = originalPrice,
                        Price = price,
                        PromotionPrice = promotionPrice,
                        Content = worksheet.Cells[i, 6].Value.ToString(),
                        SeoKeywords = worksheet.Cells[i, 7].Value.ToString(),
                        SeoDescription = worksheet.Cells[i, 8].Value.ToString(),
                        HotFlag = hotFlag,
                        HomeFlag = homeFlag,
                        Status = Data.Enums.Status.Active
                    };

                    _productRepository.Add(product);
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productViewModel)
        {
            ICollection<ProductTag> producttags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                string[] tags = productViewModel.Tags.Split(',');
                foreach (string item in tags)
                {
                    var tagId = TextHelper.ToUnsignString(item);
                    if (!_tagRepository.FillAll(x => x.Id == tagId).Any())
                    {
                        _tagRepository.Add(new Tag()
                        {
                            Name = item,
                            Id = tagId,
                            Type = CommonConstants.ProductTag
                        });
                    }
                    _productTagRepository.RemoveMulti(_productTagRepository.FillAll(x => x.ProductId == productViewModel.Id).ToList());

                    producttags.Add(new ProductTag()
                    {
                        TagId = tagId
                    });
                }
                var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
                foreach (var item in producttags)
                {
                    product.ProductTags.Add(item);
                }

                _productRepository.Update(product);
            }
        }
    }
}