using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class DeliveryService : IDeliveryService
    {
        private IRepository<Delivery, int> _deliveryRepository;
        private IUnitOfWork _unitOfWork;
        public DeliveryService(IRepository<Delivery, int> deliveryRepository, IUnitOfWork unitOfWork)
        {
            _deliveryRepository = deliveryRepository;
            _unitOfWork = unitOfWork;
        }
        public DeliveryViewModel Add(DeliveryViewModel DeliveryViewModel)
        {
            var delivery = Mapper.Map<DeliveryViewModel, Delivery>(DeliveryViewModel);
            _deliveryRepository.Add(delivery);
            _unitOfWork.Commit();
            return DeliveryViewModel;
        }

        public void Delete(int id)
        {
            _deliveryRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<DeliveryViewModel> GetAll()
        {

            var query = _deliveryRepository.FindAll();
            var data = new List<DeliveryViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Delivery, DeliveryViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<DeliveryViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<DeliveryViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public DeliveryViewModel GetById(int id)
        {
            return Mapper.Map<Delivery, DeliveryViewModel>(_deliveryRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(DeliveryViewModel DeliveryViewModel)
        {
            var temp = _deliveryRepository.FindById(DeliveryViewModel.KeyId);
            if (temp != null)
            {
                temp.DeliveryStatus = DeliveryViewModel.DeliveryStatus;
            }
        }
    }
}
