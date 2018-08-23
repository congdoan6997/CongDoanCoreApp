using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IFunctionService _functionService;
        public RoleController(IRoleService roleService, IFunctionService functionService)
        {
            _roleService = roleService;
            _functionService = functionService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page,int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var model = await _roleService.GetByIdAsync(id);
            return new OkObjectResult(model);

        }
        [HttpPost]
        public async Task<IActionResult> SaveEntityAsync(AppRoleViewModel appRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (!appRoleViewModel.Id.HasValue)
                {
                    var result = await _roleService.AddAsync(appRoleViewModel);
                    return new OkObjectResult(result);
                }
                else
                {
                    await _roleService.UpdateAsync(appRoleViewModel);
                    return new OkObjectResult(true);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(true);
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> permissionViewModels, Guid roleId)
        {
            _roleService.SavePermission(permissionViewModels, roleId);
            return new OkObjectResult(true);
        }

    }
}