using ERPWebApp.Application.ViewModels.Acc;
using ERPWebApp.Application.ViewModels.System;
using ERPWebApp.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERPWebApp.Application.Interfaces.Acc
{
    public interface IRoleService
    {
        Task<bool> AddAsync(RoleViewModel userVm);

        Task DeleteAsync(Guid id);

        Task<List<RoleViewModel>> GetAllAsync();

        PagedResult<RoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<RoleViewModel> GetById(Guid id);


        Task UpdateAsync(RoleViewModel userVm);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
