using BeYeuBookstore.Application.Interfaces.Boq;
using BeYeuBookstore.Application.ViewModels.Boq;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using BeYeuBookstore.Utilities.DTOs.Boq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation.Boq
{
    public class SO_NotificationGeneralDetailService : ISO_NotificationGeneralDetailService
    {
        private IRepository<SoNotificationGeneralDetail, int> _notificationGeneralDetailService;
        private IUnitOfWork _unitOfWork;

        public SO_NotificationGeneralDetailService(IRepository<SoNotificationGeneralDetail, int> customerLiabilityRepository, IUnitOfWork unitOfWork)
        {
            _notificationGeneralDetailService = customerLiabilityRepository;
            _unitOfWork = unitOfWork;
        }

        public SO_NotificationGeneralDetailViewModel Add(SO_NotificationGeneralDetailViewModel ItemVm)
        {
            throw new NotImplementedException();
        }

        public void AddList(List<SO_NotificationGeneralDetailViewModel> list)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SO_NotificationGeneralDetailViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<SO_NotificationGeneralDetailViewModel> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public PagedResult<SO_NotificationGeneralDTO> GetAllPaging(string keyword, int page, int pageSize,Guid userId)
        {
            var query = _notificationGeneralDetailService.FindAll(p => p.UserFk == userId);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.NotificationFkNavigation.NotificationContent.Contains(keyword));
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.Select(p => new SO_NotificationGeneralDTO()
            {
                KeyId = p.NotificationFkNavigation.KeyId,
                NotificationContent = p.NotificationFkNavigation.NotificationContent,
                NotificationTime = p.NotificationFkNavigation.DateCreated,
                Status = p.Status
            }).ToList();

            var paginationSet = new PagedResult<SO_NotificationGeneralDTO>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public SO_NotificationGeneralDetailViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<SO_NotificationGeneralDTO> getNotitficationByUserId(Guid userId)
        {
            return _notificationGeneralDetailService.FindAll(p => p.UserFk == userId).Select(p => new SO_NotificationGeneralDTO()
            {
                KeyId = p.NotificationFkNavigation.KeyId,
                NotificationContent = p.NotificationFkNavigation.NotificationContent,
                NotificationTime = p.NotificationFkNavigation.DateCreated,
                Status = p.Status
            }).ToList();
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(SO_NotificationGeneralDetailViewModel ItemVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(string userFk)
        {
            var temp = _notificationGeneralDetailService.FindAll(p =>p.UserFk.ToString()== userFk && p.Status == (int)Status.InActive).ToList();
            foreach(var item in temp)
            {
                item.Status = (int)Status.Active;
            }
        }
    }
}
