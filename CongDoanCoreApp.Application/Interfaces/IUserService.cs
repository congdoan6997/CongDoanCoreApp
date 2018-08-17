using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Utilities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel appUserViewModel);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetByIdAsync(string id);

        Task UpdateAsync(AppUserViewModel appUserViewModel);
    }
}