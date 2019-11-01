using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LACoreApp.Application.ViewModels.System;

namespace LACoreApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        void Add(FunctionViewModel function);

        Task<List<FunctionViewModel>> GetAll(string filter);

        IEnumerable<FunctionViewModel> GetAllWithParentId(Guid parentId);

        FunctionViewModel GetById(Guid id);

        void Update(FunctionViewModel function);

        void Delete(Guid id);

        void Save();

        bool CheckExistedId(Guid id);

        void UpdateParentId(Guid sourceId, Guid targetId, Dictionary<Guid, int> items);

        void ReOrder(Guid sourceId, Guid targetId);
    }
}