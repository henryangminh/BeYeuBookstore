
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Enums;
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

        PagedResult<MerchantViewModel> GetAllPaging(Status status, Scale scale, string fromdate, string todate, string keyword, int page, int pageSize);

        List<MerchantViewModel> GetAll(int id);

        MerchantViewModel GetById(int id);

        bool Save();
    }
}
