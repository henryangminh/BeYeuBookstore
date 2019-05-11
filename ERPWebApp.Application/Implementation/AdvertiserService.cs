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

        public AdvertiserViewModel GetBysId(string id)
        {
            var query = _advertiserRepository.FindAll(p => p.UserBy);
            var _data = new AdvertiserViewModel();
            foreach (var item in query)
            {
                if ((item.UserFK.ToString()) == id)
                {
                    _data = Mapper.Map<Advertiser, AdvertiserViewModel>(item);
                }
            }
            return _data;
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

        public List<AdvertiserViewModel> GetAdvertiserByStatistic(int id)
        {
            var query = _advertiserRepository.FindAll();
            if (id != 0)
            {
                query = query.Where(x => x.KeyId == id);
            }
            var data = new List<AdvertiserViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Advertiser, AdvertiserViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<AdvertiserViewModel> GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var query = _advertiserRepository.FindAll(x=>x.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => (x.BrandName.ToUpper().Contains(keysearch)));

            }
            if(status.HasValue)
            {
                query = query.Where(x => x.Status == (Status)status);
            }
          
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<AdvertiserViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Advertiser, AdvertiserViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<AdvertiserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public AdvertiserViewModel GetById(int id)
        {
            return Mapper.Map<Advertiser, AdvertiserViewModel>(_advertiserRepository.FindById(id,p=>p.UserBy));
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
