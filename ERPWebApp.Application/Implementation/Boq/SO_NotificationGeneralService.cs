using AutoMapper;
using AutoMapper.QueryableExtensions;
using BeYeuBookstore.Application.Interfaces.Boq;
using BeYeuBookstore.Application.ViewModels.Boq;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation.Boq
{
    public class SO_NotificationGeneralService : ISO_NotificationGeneralService
    {
        private IRepository<SoNotificationGeneral, int> _notificationGeneralService;
        private IUnitOfWork _unitOfWork;

        public SO_NotificationGeneralService(IRepository<SoNotificationGeneral, int> errorLogRepository, IUnitOfWork unitOfWork)
        {
            _notificationGeneralService = errorLogRepository;
            _unitOfWork = unitOfWork;
        }

        public SO_NotificationGeneralViewModel Add(SO_NotificationGeneralViewModel ItemVm)
        {
            var Item = Mapper.Map<SO_NotificationGeneralViewModel, SoNotificationGeneral>(ItemVm);
            _notificationGeneralService.Add(Item);
            return ItemVm;
        }

        public void AddList(List<SO_NotificationGeneralViewModel> list)
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

        public List<SO_NotificationGeneralViewModel> GetAll()
        {
			var query = _notificationGeneralService.FindAll().OrderBy(p => p.KeyId);
			var data = new List<SO_NotificationGeneralViewModel>();
			foreach (var item in query)
			{
				var _data = Mapper.Map<SoNotificationGeneral, SO_NotificationGeneralViewModel>(item);
				data.Add(_data);
			}
			return data;

			//return _notificationGeneralService.FindAll().OrderBy(p => p.KeyId) // list ra va sx theo Id
   //             .ProjectTo<SO_NotificationGeneralViewModel>().ToList();
        }

        public List<SO_NotificationGeneralViewModel> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public PagedResult<SO_NotificationGeneralViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public SO_NotificationGeneralViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(SO_NotificationGeneralViewModel ItemVm)
        {
            throw new NotImplementedException();
        }
    }
}
