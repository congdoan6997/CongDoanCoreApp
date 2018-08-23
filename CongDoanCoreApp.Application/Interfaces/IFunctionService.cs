using CongDoanCoreApp.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CongDoanCoreApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task<List<FunctionViewModel>> GetAll(string filter);

        List<FunctionViewModel> GetAllByPermission(Guid userId);

        void Add(FunctionViewModel functionViewModel);

        FunctionViewModel GetById(string id);

        void Update(FunctionViewModel functionViewModel);

        void Delete(string id);

        void Save();

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);
    }
}