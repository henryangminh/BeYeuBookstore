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
    public class AdvertisementPositionService : IAdvertisementPositionService
    {
        private IRepository<AdvertisementPosition, int> _advertisementPositionRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertisementPositionService(IRepository<AdvertisementPosition, int> advertisementPositionRepository, IUnitOfWork unitOfWork)
        {
            _advertisementPositionRepository = advertisementPositionRepository;
            _unitOfWork = unitOfWork;

        }
        public AdvertisementPositionViewModel Add(AdvertisementPositionViewModel AdvertisementPositionViewModel)
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

        public List<AdvertisementPositionViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<AdvertisementPositionViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AdvertisementPositionViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public AdvertisementPositionViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(AdvertisementPositionViewModel AdvertisementPositionViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
