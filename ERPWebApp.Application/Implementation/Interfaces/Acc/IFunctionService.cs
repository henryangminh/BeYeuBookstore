using BeYeuBookstore.Application.ViewModels.Acc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Application.Interfaces.Acc
{
    public interface IFunctionService : IDisposable
    {
        void Add(FunctionViewModel function);

        Task<List<FunctionViewModel>> GetAll(string filter);
        List<FunctionViewModel> GetAllByRoles(string roles);

        IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId);

        FunctionViewModel GetById(string id);

        void Update(FunctionViewModel function);

        void Delete(string id);

        bool Save();

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);
    }
}
