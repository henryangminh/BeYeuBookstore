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
            var advertisementContent = Mapper.Map<AdvertisementContentViewModel, AdvertisementContent>(AdvertisementContentViewModel);
            _advertisementContentRepository.Add(advertisementContent);
            _unitOfWork.Commit();
            return AdvertisementContentViewModel;
        }

        public void Delete(int id)
        {
            _advertisementContentRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<AdvertisementContentViewModel> GetAll()
        {
            var query = _advertisementContentRepository.FindAll();
            var data = new List<AdvertisementContentViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertisementContent, AdvertisementContentViewModel>(item);
                data.Add(_data);
            }
            return data;
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
            return Mapper.Map<AdvertisementContent, AdvertisementContentViewModel>(_advertisementContentRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(AdvertisementContentViewModel AdvertisementContentViewModel)
        {
            var temp = _advertisementContentRepository.FindById(AdvertisementContentViewModel.KeyId);
            if (temp != null)
            {

                temp.UrlToAdvertisement = AdvertisementContentViewModel.UrlToAdvertisement;
                temp.Title = AdvertisementContentViewModel.Title;
                temp.ImageLink = AdvertisementContentViewModel.ImageLink;
                temp.PaidDeposite = AdvertisementContentViewModel.PaidDeposite;
                temp.Description = AdvertisementContentViewModel.Description;
                temp.Deposite = AdvertisementContentViewModel.Deposite;
                temp.CensorStatus = AdvertisementContentViewModel.CensorStatus;
            }
        }
    }
}
