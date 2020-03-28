using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LACoreApp.Application.Interfaces
{
    public interface IMenuService : IDisposable
    {

        void Add(MenuViewModel menu);

        Task<List<MenuViewModel>> GetList(string filter, Guid? parentId = null, int? group = null, Status? status = null);

        IEnumerable<MenuViewModel> GetAllWithParentId(Guid parentId);

        MenuViewModel GetById(Guid id);

        void Update(MenuViewModel menu);

        void Delete(Guid id);

        void Save();

        bool CheckExistedId(Guid id);

        void UpdateParentId(Guid sourceId, Guid targetId, Dictionary<Guid, int> items);

        void ReOrder(Guid sourceId, Guid targetId);
    }
}
