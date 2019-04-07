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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<DeliveryViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(DeliveryViewModel DeliveryViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
