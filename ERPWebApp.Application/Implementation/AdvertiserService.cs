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
    public class AdvertiserService : IAdvertiserService
    {
        private IRepository<Advertiser, int> _advertiserRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertiserService(IRepository<Advertiser, int> advertiserRepository, IUnitOfWork unitOfWork)
        {
            _advertiserRepository = advertiserRepository;
            _unitOfWork = unitOfWork;

        }

        public AdvertiserViewModel Add(AdvertiserViewModel AdvertiserViewModel)
        {
            var advertiser = Mapper.Map<AdvertiserViewModel, Advertiser>(AdvertiserViewModel);
            _advertiserRepository.Add(advertiser);
            _unitOfWork.Commit();
            return AdvertiserViewModel;
        }

        public void Delete(int id)
        {
            _advertiserRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<AdvertiserViewModel> GetAll()
        {
            var query = _advertiserRepository.FindAll();
            var data = new List<AdvertiserViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Advertiser, AdvertiserViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<AdvertiserViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AdvertiserViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public AdvertiserViewModel GetById(int id)
        {
            return Mapper.Map<Advertiser, AdvertiserViewModel>(_advertiserRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(AdvertiserViewModel AdvertiserViewModel)
        {
            var temp = _advertiserRepository.FindById(AdvertiserViewModel.KeyId);
            if (temp != null)
            {
                temp.BrandName = AdvertiserViewModel.BrandName;
                temp.Status = AdvertiserViewModel.Status;
                temp.UrlToBrand = AdvertiserViewModel.UrlToBrand;
            }
        }
    }
}
