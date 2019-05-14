using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IAdvertiserService : IDisposable
    {
        AdvertiserViewModel Add(AdvertiserViewModel AdvertiserViewModel);

        void Update(AdvertiserViewModel AdvertiserViewModel);

        void Delete(int id);

        List<AdvertiserViewModel> GetAll();

        PagedResult<AdvertiserViewModel> GetAllPaging(int? status, string keyword, int page, int pageSize);

        List<AdvertiserViewModel> GetAdvertiserByStatistic(int id);

        AdvertiserViewModel GetById(int id);

        AdvertiserViewModel GetBysId(string id);
        bool Save();
    }
}
