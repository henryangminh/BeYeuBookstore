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
    public class AdvertisementContentService : IAdvertisementContentService
    {
        private IRepository<AdvertisementContent, int> _advertisementContentRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertisementContentService(IRepository<AdvertisementContent, int> advertisementContentRepository, IUnitOfWork unitOfWork)
        {
            _advertisementContentRepository = advertisementContentRepository;
            _unitOfWork = unitOfWork;

        }
        public AdvertisementContentViewModel Add(AdvertisementContentViewModel AdvertisementContentViewModel)
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

        public List<AdvertisementContentViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<AdvertisementContentViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AdvertisementContentViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public AdvertisementContentViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(AdvertisementContentViewModel AdvertisementContentViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
