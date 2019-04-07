using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface ICustomerService : IDisposable
    {
        CustomerViewModel Add(CustomerViewModel CustomerViewModel);

        void Update(CustomerViewModel CustomerViewModel);

        void Delete(int id);

        List<CustomerViewModel> GetAll();

        PagedResult<CustomerViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<CustomerViewModel> GetAll(int id);

        CustomerViewModel GetById(int id);

        bool Save();
    }
}
