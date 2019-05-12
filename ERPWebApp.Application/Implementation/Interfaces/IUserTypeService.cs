using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IUserTypeService : IDisposable
    {
        UserTypeViewModel Add(UserTypeViewModel UserTypeViewModel);

        void Update(UserTypeViewModel UserTypeViewModel);

        void Delete(int id);

        List<UserTypeViewModel> GetAll();

        PagedResult<UserTypeViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<UserTypeViewModel> GetAll(int id);

        UserTypeViewModel GetById(int id);

        bool Save();
    }
}
