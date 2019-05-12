
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IWebMasterTypeService : IDisposable
    {
        WebMasterTypeViewModel Add(WebMasterTypeViewModel WebMasterTypeViewModel);

        void Update(WebMasterTypeViewModel WebMasterTypeViewModel);

        void Delete(int id);

        List<WebMasterTypeViewModel> GetAll();

        PagedResult<WebMasterTypeViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<WebMasterTypeViewModel> GetAll(int id);

        WebMasterTypeViewModel GetById(int id);

        bool Save();
    }
}
