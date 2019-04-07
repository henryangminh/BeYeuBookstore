using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IWebMasterService : IDisposable
    {
        WebMasterViewModel Add(WebMasterViewModel WebMasterViewModel);

        void Update(WebMasterViewModel WebMasterViewModel);

        void Delete(int id);

        List<WebMasterViewModel> GetAll();

        PagedResult<WebMasterViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<WebMasterViewModel> GetAll(int id);

        WebMasterViewModel GetById(int id);

        bool Save();
    }
}

