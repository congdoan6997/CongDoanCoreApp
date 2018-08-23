using AutoMapper;
using AutoMapper.QueryableExtensions;
using CongDoanCoreApp.Application.Interfaces;
using CongDoanCoreApp.Application.ViewModels.System;
using CongDoanCoreApp.Data.Entities;
using CongDoanCoreApp.Data.IRepositories;
using CongDoanCoreApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<FunctionViewModel> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FillAll();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter) || x.Id.Contains(filter));
            }
            return query.ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public void Add(FunctionViewModel functionViewModel)
        {
            var function = Mapper.Map<FunctionViewModel, Function>(functionViewModel);
            _functionRepository.Add(function);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindById(id);
            return Mapper.Map<Function, FunctionViewModel>(function);
        }

        public void Update(FunctionViewModel functionViewModel)
        {
            var function = Mapper.Map<FunctionViewModel, Function>(functionViewModel);
            _functionRepository.Update(function);
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //get all sibling
            var sibling = _functionRepository.FillAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public void ReOrder(string sourceId, string targetId)
        {
            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            var temp = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = temp;

            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }
    }
}