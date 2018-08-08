using CongDoanCoreApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private readonly UserManager<AppUser> _userManager;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            this._userManager = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser appUser)
        {
            var pricipal = await base.CreateAsync(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);
            ((ClaimsIdentity)pricipal.Identity).AddClaims(new[]
            {
                new Claim("Email", appUser.Email),
                new Claim("Avatar", appUser.Avatar?? string.Empty),
                new Claim("FullName", appUser.FullName),
                new Claim("Roles", string.Join(";",roles))
            });
            return pricipal;
        }
    }
}