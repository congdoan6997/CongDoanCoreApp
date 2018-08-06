using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Models.AccountViewModels;
using CongDoanCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Areas.Admin.Controllers
{
    [Area("admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<LoginController> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model, string returnUrl = null)
        {
            
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return new OkObjectResult(new GenericResult(true));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new ObjectResult(new GenericResult(false, "Tài khoản bị khóa"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return new ObjectResult(new GenericResult(false, "Đăng nhập sai"));
                }
            }

            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(false, model));
        }
    }
}