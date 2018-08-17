using AutoMapper;
using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AddAsync(AppUserViewModel appUserViewModel)
        {
            var user = new AppUser()
            {
                UserName = appUserViewModel.UserName,
                Avatar = appUserViewModel.Avatar,
                Email = appUserViewModel.Email,
                FullName = appUserViewModel.FullName,
                PhoneNumber = appUserViewModel.PhoneNumber,
                Status = appUserViewModel.Status,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, appUserViewModel.Password);
            if (result.Succeeded && appUserViewModel.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                {
                    await _userManager.AddToRolesAsync(appUser, appUserViewModel.Roles);
                }
                return true;
            }
            return false;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
        }

        public PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));
            }
            var totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            //var data = query.Select(x => new AppUserViewModel()
            //{
            //    UserName = x.UserName,
            //    Avatar = x.Avatar,
            //    BirthDay = x.BirthDay,
            //    Email = x.Email,
            //    FullName = x.FullName,
            //    Id = x.Id,
            //    PhoneNumber = x.PhoneNumber,
            //    Status = x.Status,
            //    DateCreated = x.DateCreated
            //}).ToList();
            var data = query.ProjectTo<AppUserViewModel>().ToList();
            var paginationSet = new PageResult<AppUserViewModel>()
            {
                Results = data,
                PageSize = pageSize,
                CurrentPage = page,
                RowCount = totalRow
            };
            return paginationSet;
        }

        public async Task<AppUserViewModel> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = Mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }

        public async Task UpdateAsync(AppUserViewModel appUserViewModel)
        {
            var user = await _userManager.FindByIdAsync(appUserViewModel.Id.ToString());
            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, appUserViewModel.Roles.Except(currentRoles).ToArray());
            if (result.Succeeded)
            {
                var needRemoveRoles = currentRoles.Except(appUserViewModel.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                user.FullName = appUserViewModel.FullName;
                user.Status = appUserViewModel.Status;
                user.Email = appUserViewModel.Email;
                user.PhoneNumber = appUserViewModel.PhoneNumber;
                user.DateModified = DateTime.Now;

                await _userManager.UpdateAsync(user);
            }
        }
    }
}