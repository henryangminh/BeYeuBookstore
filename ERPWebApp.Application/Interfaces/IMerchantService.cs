
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IMerchantService : IDisposable
    {
        MerchantViewModel Add(MerchantViewModel MerchantViewModel);

        void Update(MerchantViewModel MerchantViewModel);

        void Delete(int id);

        List<MerchantViewModel> GetAll();

        PagedResult<MerchantViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<MerchantViewModel> GetAll(int id);

        MerchantViewModel GetById(int id);

        bool Save();
    }
}
