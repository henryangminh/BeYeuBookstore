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
   public interface IAdvertisementContentService : IDisposable
    {
        AdvertisementContentViewModel Add(AdvertisementContentViewModel AdvertisementContentViewModel);

        void Update(AdvertisementContentViewModel AdvertisementContentViewModel);

        void Delete(int id);

        List<AdvertisementContentViewModel> GetAll();

        PagedResult<AdvertisementContentViewModel> GetAllPaging(int? advertiserId, int? status, string keyword, int page, int pageSize);

        List<AdvertisementContentViewModel> GetAll(int id);

        AdvertisementContentViewModel GetById(int id);

        void UpdateStatus(int id, int censorFK, int status);
        bool Save();
    }
}
