using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Area("admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}