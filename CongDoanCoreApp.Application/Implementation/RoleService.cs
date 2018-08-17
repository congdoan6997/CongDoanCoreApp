using AutoMapper;
using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;
using CongDoanCoreApp.Infrastructure.Interfaces;
using CongDoanCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork
            , IFunctionRepository functionRepository, IPermissionRepository permissionRepository)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> AddAsync(AppRoleViewModel appRoleViewModel)
        {
            var role = Mapper.Map<AppRoleViewModel, AppRole>(appRoleViewModel);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public Task<bool> CheckPermissionAsync(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FillAll();
            var permissions = _permissionRepository.FillAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }

        public PageResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword)
               || x.Description.Contains(keyword));
            }
            var totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<AppRoleViewModel>().ToList();
            var paginationSet = new PageResult<AppRoleViewModel>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
            return paginationSet;
        }

        public async Task<AppRoleViewModel> GetByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return Mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FillAll();
            var permissions = _permissionRepository.FillAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null ? p.CanCreate : false,
                            CanDelete = p != null ? p.CanDelete : false,
                            CanRead = p != null ? p.CanRead : false,
                            CanUpdate = p != null ? p.CanUpdate : false
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionViewModels, Guid roleId)
        {
            var permissions = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionViewModels);
            var oldPermission = _permissionRepository.FillAll().Where(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.RemoveMulti(oldPermission);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel appRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(appRoleViewModel.Id.ToString());
            role.Description = appRoleViewModel.Description;
            role.Name = appRoleViewModel.Name;
            await _roleManager.UpdateAsync(role);
        }
    }
}