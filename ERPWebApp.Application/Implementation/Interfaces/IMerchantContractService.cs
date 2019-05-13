using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IMerchantContractService : IDisposable
    {
        MerchantContractViewModel Add(MerchantContractViewModel MerchantContractViewModel);

        void Update(MerchantContractViewModel MerchantContractViewModel);

        void Delete(int id);

        List<MerchantContractViewModel> GetAll();

        PagedResult<MerchantContractViewModel> GetAllPaging(int? merchantId, string fromdate, string todate, string keyword, int page, int pageSize);

        List<MerchantContractViewModel> GetAll(int id);

        MerchantContractViewModel GetById(int id);

        bool Save();
    }
}
