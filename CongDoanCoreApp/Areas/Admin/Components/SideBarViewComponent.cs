using CongDoanCoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CongDoanCoreApp.Extensions;
using System.Security.Claims;
using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Utilities.Constants;

namespace CongDoanCoreApp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;
        public SideBarViewComponent(IFunctionService functionService)
        {
            this._functionService = functionService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;

            if (roles.Split(";").Contains(CommonConstants.AdminRole))
            {
                functions = await  _functionService.GetAll();
            }
            else
            {
                //todo: get by permission
                functions = new List<FunctionViewModel>();
            }
            return View(functions);
        }
        
    }
}
