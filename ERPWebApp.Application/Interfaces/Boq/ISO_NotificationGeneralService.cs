using BeYeuBookstore.Application.ViewModels.Boq;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces.Boq
{
    public interface ISO_NotificationGeneralService:IDisposable
    {
        List<SO_NotificationGeneralViewModel> GetAll();
        SO_NotificationGeneralViewModel Add(SO_NotificationGeneralViewModel ItemVm);

        void AddList(List<SO_NotificationGeneralViewModel> list);

        void Update(SO_NotificationGeneralViewModel ItemVm);

        void Delete(int id);

        PagedResult<SO_NotificationGeneralViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<SO_NotificationGeneralViewModel> GetAll(string keyword);

        SO_NotificationGeneralViewModel GetById(int id);

        bool Save();
    }
}
