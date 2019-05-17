using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IAdvertiseContractService : IDisposable
    {
        AdvertiseContractViewModel Add(AdvertiseContractViewModel advertiseContractViewModel);

        void Update(AdvertiseContractViewModel advertiseContractViewModel);
        void UpdateStatus(int id, int status, string note);

        void Delete(int id);

        List<AdvertiseContractViewModel> GetAll();

        List<AdvertiseContractViewModel> GetAllRequestingNPaidContract();

        PagedResult<AdvertiseContractViewModel> GetAllPaging(string fromdate, string todate, bool? isSaleAdmin, bool? isAccountant, int advertiserId, int? status, string keyword, int page, int pageSize);

        PagedResult<AdStatisticViewModel> GetAllStatisticPaging(string fromdate, string todate, int advertiserId, int page, int pageSize);

        List<AdvertiseContractViewModel> GetAllFutureContractByPositionId(int id);
        List<AdvertiseContractViewModel> GetAllContractByPositionId(int id);
        AdvertiseContractViewModel GetById(int id);

        bool Save();
    }
}
