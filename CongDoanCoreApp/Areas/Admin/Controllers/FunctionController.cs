using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllFilter(string filter)
        {
            var model = _functionService.GetAll(filter);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll(string.Empty);

            var rootFunctions = model.Where(x => x.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new OkObjectResult(items);
        }

        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = _functionService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel functionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (!_functionService.CheckExistedId(functionViewModel.Id))
                {
                    _functionService.Add(functionViewModel);
                }
                else
                {
                    _functionService.Update(functionViewModel);
                }
                _functionService.Save();
                return new OkObjectResult(functionViewModel);
            }
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (id == null)
                {
                    return new BadRequestObjectResult("Id cannot 0");
                }
                else
                {
                    _functionService.Delete(id);
                    _functionService.Save();
                    return new OkObjectResult(true);
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestObjectResult(false);
                }
                else
                {
                    _functionService.UpdateParentId(sourceId, targetId, items);
                    _functionService.Save();
                    return new OkObjectResult(true);
                }
            }
        }

        public IActionResult ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _functionService.ReOrder(sourceId, targetId);
                    _functionService.Save();
                    return new OkObjectResult(true);
                }
            }
        }

        #region private Functions

        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions, FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();

            var subFunctions = functionEntities.Where(x => x.ParentId == parent.Id);

            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //resursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionEntities, cat, items);
            }
        }

        #endregion private Functions
    }
}