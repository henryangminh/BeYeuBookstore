
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
        List<MerchantViewModel> GetAllByBook(int id);

        PagedResult<MerchantViewModel> GetAllPaging(int? status, int? scale, string fromdate, string todate, string keyword, int page, int pageSize);

        List<MerchantViewModel> GetAll(int id);

        MerchantViewModel GetById(int id);

        MerchantViewModel GetBysId(string id);

        bool Save();
    }
}
