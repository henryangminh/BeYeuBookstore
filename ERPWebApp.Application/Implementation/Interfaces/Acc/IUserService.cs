using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Application.Interfaces.Acc
{ 
    public interface IUserService: IDisposable
    {
        Task<string> GetFullName(string Id);

        Task<bool> AddAsync(UserViewModel userVm);
        Task<bool> AddAddressBookAsync(UserViewModel userVm);
        Task DeleteAsync(string id);

        Task<List<UserViewModel>> GetAllAsync();

        PagedResult<UserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<UserViewModel> GetById(string id);
        Task<List<string>> GetAllRolesById(string Id);
        Task<UserViewModel> GetByIdAddressbook(string id);
        //Task<List<IdAndName>> GetListIdAndFullName(bool status, string type);
        Task<bool> UpdateAsync(UserViewModel userVm);
        Task<bool> UpdateByCustomer(UserViewModel userVm);
        Task<bool> UpdateAddressBookAsync(UserViewModel userVm);
        Task<List<Guid>> getListUserId(string roleName);
        bool checkUserInRole(string userid, string roleName);
        bool checkExistEmail(string email);

        Task<User> GetByEmailAsync(string email);
    }
}
