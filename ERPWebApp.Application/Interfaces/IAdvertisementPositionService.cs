using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;


namespace BeYeuBookstore.Application.Interfaces
{
   public interface IAdvertisementPositionService : IDisposable
    {
        AdvertisementPositionViewModel Add(AdvertisementPositionViewModel AdvertisementPositionViewModel);

        void Update(AdvertisementPositionViewModel AdvertisementPositionViewModel);

        void Delete(int id);

        List<AdvertisementPositionViewModel> GetAll();

        PagedResult<AdvertisementPositionViewModel> GetAllPaging(int? status, string keyword, int page, int pageSize);

        List<AdvertisementPositionViewModel> GetAll(int id);

        AdvertisementPositionViewModel GetById(int id);

        bool Save();
    }
    
}
