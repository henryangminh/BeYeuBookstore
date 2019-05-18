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
    public class MerchantService : IMerchantService
    {
        private IRepository<Merchant, int> _merchantRepository;
        private IUnitOfWork _unitOfWork;
        public MerchantService(IRepository<Merchant, int> merchantRepository, IUnitOfWork unitOfWork)
        {
            _merchantRepository = merchantRepository;
            _unitOfWork = unitOfWork;
        }
        public MerchantViewModel Add(MerchantViewModel MerchantViewModel)
        {
            var Merchant = Mapper.Map<MerchantViewModel, Merchant>(MerchantViewModel);
            _merchantRepository.Add(Merchant);
            _unitOfWork.Commit();
            return MerchantViewModel;
        }

        public void Delete(int id)
        {
            _merchantRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<MerchantViewModel> GetAll()
        {
            var query = _merchantRepository.FindAll(x => x.UserBy);
            var data = new List<MerchantViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Merchant, MerchantViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<MerchantViewModel> GetAllByBook(int id)
        {
            var query = _merchantRepository.FindAll();
            if(id!=0)
            {
                query = query.Where(x => x.KeyId == id);
            }
            var data = new List<MerchantViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Merchant, MerchantViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public MerchantViewModel GetBysId(string id)
        {
            var query = _merchantRepository.FindAll(p => p.UserBy);
            var _data = new MerchantViewModel();
            foreach (var item in query)
            {
                if ((item.UserFK.ToString()) == id)
                {
                    _data = Mapper.Map<Merchant, MerchantViewModel>(item);

                }
            }
            return _data;

        }

        public List<MerchantViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<MerchantViewModel> GetAllPaging(int? status, int? scale, string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _merchantRepository.FindAll(p => p.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();
                query = query.OrderBy(x => x.KeyId).Where(x => ((x.MerchantCompanyName.ToUpper().Contains(keysearch)) || x.DirectContactName.ToUpper().Contains(keysearch)));

            }
            if (!string.IsNullOrEmpty(fromdate))
            {
                var date = DateTime.Parse(fromdate);
                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime _fromdate = date.Date + ts;
                query = query.Where(x => x.DateCreated >= _fromdate);

            }
            if (!string.IsNullOrEmpty(todate))
            {
                var date = DateTime.Parse(todate);
                TimeSpan ts = new TimeSpan(23, 59, 59);
                DateTime _todate = date.Date + ts;
                query = query.Where(x => x.DateCreated <= _todate);

            }
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == (Status)status);
            }

            if (scale.HasValue)
            {
                query = query.Where(x => x.Scales == (Scale)scale);
            }

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<MerchantViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Merchant, MerchantViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<MerchantViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public MerchantViewModel GetById(int id)
        {
            return Mapper.Map<Merchant, MerchantViewModel>(_merchantRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(MerchantViewModel MerchantViewModel)
        {
            var temp = _merchantRepository.FindById(MerchantViewModel.KeyId);
            if (temp != null)
            {
                temp.Bank = MerchantViewModel.Bank;
            }
        }
    }
}

