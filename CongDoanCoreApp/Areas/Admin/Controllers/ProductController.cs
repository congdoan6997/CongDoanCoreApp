using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CongDoanCoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            this._productService = productService;
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
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page,int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId,keyword,page,pageSize);
            return new OkObjectResult(model);
        }
        #endregion
    }
}