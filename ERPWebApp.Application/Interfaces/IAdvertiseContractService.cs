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

        void Delete(int id);

        List<AdvertiseContractViewModel> GetAll();

        PagedResult<AdvertiseContractViewModel> GetAllPaging(int advertiserId, int? status, string keyword, int page, int pageSize);

        List<AdvertiseContractViewModel> GetAll(int id);

        AdvertiseContractViewModel GetById(int id);

        bool Save();
    }
}
