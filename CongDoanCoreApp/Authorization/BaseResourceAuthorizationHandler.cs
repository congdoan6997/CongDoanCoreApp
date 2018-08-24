using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Authorization
{
    public class BaseResourceAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        private readonly IRoleService _roleService;

        public BaseResourceAuthorizationHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, string resource)
        {
            var roles = ((ClaimsIdentity)context.User.Identity).Claims.FirstOrDefault(x => x.Type == CommonConstants.UserClaims.Roles);
            if (roles != null)
            {
                var listRoles = roles.Value.Split(";");
                var hasPermission = await _roleService.CheckPermissionAsync(resource, requirement.Name, listRoles);
                if (hasPermission || listRoles.Contains(CommonConstants.AppRole.AdminRole))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}