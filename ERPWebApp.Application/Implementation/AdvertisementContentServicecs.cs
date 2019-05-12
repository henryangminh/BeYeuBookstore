using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<AdvertisementContentViewModel> GetAllCensoredAdContentByAdvertiserId(int id)
        {

            var query = _advertisementContentRepository.FindAll(x=>(x.AdvertiserFK==id && x.CensorStatus == CensorStatus.AccountingCensored && x.AdvertiseContract==null ));
            var data = new List<AdvertisementContentViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertisementContent, AdvertisementContentViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<AdvertisementContentViewModel> GetAllPaging(bool? isAdCensor, bool? isAccountant, int? advertiserId, int? status, string keyword, int page, int pageSize)
        {
            var query = _advertisementContentRepository.FindAll(x=>x.AdvertisementPositionFKNavigation, x=>x.AdvertiserFKNavigation, x=>x.WebMasterCensorFKNavigation.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => (x.Title.ToUpper().Contains(keysearch)|| x.AdvertiserFKNavigation.BrandName.ToUpper().Contains(keysearch) || x.Description.ToUpper().Contains(keysearch)));

            }
            if (status.HasValue)
            {
                query = query.Where(x => x.CensorStatus == (CensorStatus)status);
            }
            if (advertiserId!=0)
            {
                query = query.Where(x => x.AdvertiserFK == advertiserId);
            }
            if (isAccountant == true)
            {
                query = query.Where(x => (x.CensorStatus == CensorStatus.ContentCensored || x.CensorStatus == CensorStatus.AccountingCensored));
            }
            if (isAdCensor == true)
            {
                query = query.Where(x => (x.CensorStatus == CensorStatus.Uncensored|| x.CensorStatus == CensorStatus.Unqualified));
            }
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<AdvertisementContentViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertisementContent, AdvertisementContentViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<AdvertisementContentViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public AdvertisementContentViewModel GetById(int id)
        {
            return Mapper.Map<AdvertisementContent, AdvertisementContentViewModel>(_advertisementContentRepository.FindById(id, x => x.AdvertisementPositionFKNavigation, x => x.AdvertiserFKNavigation, x => x.WebMasterCensorFKNavigation.UserBy));
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
                temp.Description = AdvertisementContentViewModel.Description;
                temp.Deposite = AdvertisementContentViewModel.Deposite;
                
            }
        }
        public void UpdateStatus(int id, int censorFK, int status, string note)
        {
            var temp = _advertisementContentRepository.FindById(id);
            if (temp != null)
            {
                temp.CensorStatus = (CensorStatus)status;
                temp.CensorFK = censorFK;
                temp.Note = note;
            }

        }
    }
}
