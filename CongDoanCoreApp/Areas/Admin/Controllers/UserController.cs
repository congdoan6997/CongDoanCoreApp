using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region ajax api

        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var model = await _userService.GetByIdAsync(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntityAsync(AppUserViewModel appUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (appUserViewModel.Id == null)
                {
                   var result =  await _userService.AddAsync(appUserViewModel);
                    if (result)
                    {
                        return new OkObjectResult(true);
                    }
                    return new BadRequestObjectResult(false);
                }
                else
                {
                    await _userService.UpdateAsync(appUserViewModel);
                }
                return new OkObjectResult(appUserViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new BadRequestObjectResult(id);
                }
                else
                {
                    await _userService.DeleteAsync(id);

                    return new OkObjectResult(true);
                }
            }
        }

        #endregion ajax api
    }
}