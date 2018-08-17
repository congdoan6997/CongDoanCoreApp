using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel appRoleViewModel);

        Task DeleteAsync(string id);

        Task<List<AppRoleViewModel>> GetAllAsync();

        PageResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetByIdAsync(string id);

        Task UpdateAsync(AppRoleViewModel appRoleViewModel);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissionViewModels, Guid roleId);

        Task<bool> CheckPermissionAsync(string functionId, string action, string[] roles);
    }
}