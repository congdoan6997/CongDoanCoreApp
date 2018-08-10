using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CongDoanCoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region get data API
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }
        #endregion
    }
}