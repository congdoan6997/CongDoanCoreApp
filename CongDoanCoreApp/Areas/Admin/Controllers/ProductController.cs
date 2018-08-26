using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.Product;
using CongDoanCoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IHostingEnvironment hostingEnvironment)
        {
            this._productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region ajax api

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (id == 0)
                {
                    return new BadRequestObjectResult("id cannot 0");
                }
                _productService.Delete(id);
                _productService.Save();
                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productviewmodel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                productviewmodel.SeoAlias = TextHelper.ToUnsignString(productviewmodel.Name);

                if (productviewmodel.Id == 0)
                {
                    _productService.Add(productviewmodel);
                }
                else
                {
                    _productService.Update(productviewmodel);
                }
                _productService.Save();
                return new OkObjectResult(productviewmodel);
            }
        }
        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                    .FileName.Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                _productService.ImportExcel(filePath, categoryId);
                _productService.Save();
                return new OkObjectResult(true);
            }
            return new NoContentResult();
        }

        #endregion ajax api
    }
}