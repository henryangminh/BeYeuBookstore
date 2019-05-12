using BeYeuBookstore.Application.ViewModels.Boq;
using BeYeuBookstore.Utilities.DTOs;
using BeYeuBookstore.Utilities.DTOs.Boq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Interfaces.Boq
{
    public interface ISO_NotificationGeneralDetailService:IDisposable
    {
        List<SO_NotificationGeneralDetailViewModel> GetAll();
        SO_NotificationGeneralDetailViewModel Add(SO_NotificationGeneralDetailViewModel ItemVm);

        void AddList(List<SO_NotificationGeneralDetailViewModel> list);

        void Update(SO_NotificationGeneralDetailViewModel ItemVm);

        void Delete(int id);

        PagedResult<SO_NotificationGeneralDTO> GetAllPaging(string keyword, int page, int pageSize,Guid userId);

        List<SO_NotificationGeneralDetailViewModel> GetAll(string keyword);

        SO_NotificationGeneralDetailViewModel GetById(int id);

        List<SO_NotificationGeneralDTO> getNotitficationByUserId(Guid userId);

        void UpdateStatus(string userFk);

        bool Save();

    }
}
