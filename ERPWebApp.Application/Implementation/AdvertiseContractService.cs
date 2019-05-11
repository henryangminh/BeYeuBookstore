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
    public class AdvertiseContractService : IAdvertiseContractService
    {
        private IRepository<AdvertiseContract, int> _advertiseContractRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertiseContractService(IRepository<AdvertiseContract, int> advertiseContractRepository, IUnitOfWork unitOfWork)
        {
            _advertiseContractRepository = advertiseContractRepository;
            _unitOfWork = unitOfWork;

        }
        public AdvertiseContractViewModel Add(AdvertiseContractViewModel advertiseContractViewModel)
        {
            var AdvertiseContract = Mapper.Map<AdvertiseContractViewModel, AdvertiseContract>(advertiseContractViewModel);
            _advertiseContractRepository.Add(AdvertiseContract);
            _unitOfWork.Commit();
            return advertiseContractViewModel;
        }

        public void Delete(int id)
        {
            _advertiseContractRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<AdvertiseContractViewModel> GetAll()
        {
            var query = _advertiseContractRepository.FindAll();
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<AdvertiseContractViewModel> GetAllRequestingNPaidContract()
        {
            var query = _advertiseContractRepository.FindAll(x=>(x.Status==ContractStatus.Requesting|| x.Status == ContractStatus.AccountingCensored));
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<AdvertiseContractViewModel> GetAllFutureContractByPositionId(int id)
        {
            var query = _advertiseContractRepository.FindAll(x=>(x.Status == ContractStatus.Requesting && x.DateFinish >= DateTime.Now && x.AdvertisementContentFKNavigation.AdvertisementPositionFK==id));
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<AdvertiseContractViewModel> GetAllPaging(string fromdate, string todate, bool? isSaleAdmin, bool? isAccountant, int advertiserId, int? status, string keyword, int page, int pageSize)
        {
            var query = _advertiseContractRepository.FindAll(x => x.AdvertisementContentFKNavigation, x=>x.AdvertisementContentFKNavigation.AdvertiserFKNavigation);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => (x.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName.ToUpper().Contains(keysearch) || x.AdvertisementContentFKNavigation.Title.ToUpper().Contains(keysearch)));

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
                query = query.Where(x => x.Status == (ContractStatus)status);
            }
            if (advertiserId != 0)
            {
                query = query.Where(x => x.AdvertisementContentFKNavigation.AdvertiserFK == advertiserId);
            }
            if (isAccountant == true)
            {
                query = query.Where(x => (x.Status == ContractStatus.Requesting));
            }
            if (isSaleAdmin == true)
            {
                query = query.Where(x => (x.Status == ContractStatus.AccountingCensored || x.Status == ContractStatus.Unqualified || x.Status == ContractStatus.Success));
            }
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<AdvertiseContractViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public PagedResult<AdvertiseContractViewModel> GetAllStatisticPaging(string frommonth, int advertiserId, int page, int pageSize)
        {
            var query = _advertiseContractRepository.FindAll(x => x.AdvertisementContentFKNavigation, x => x.AdvertisementContentFKNavigation.AdvertiserFKNavigation);
            query = query.OrderBy(x => x.KeyId);

            //if (!string.IsNullOrEmpty(fromdate))
            //{
            //    var date = DateTime.Parse(fromdate);
            //    TimeSpan ts = new TimeSpan(0, 0, 0);
            //    DateTime _fromdate = date.Date + ts;
            //    query = query.Where(x => x.DateCreated >= _fromdate);

            //}
            //if (!string.IsNullOrEmpty(todate))
            //{
            //    var date = DateTime.Parse(todate);
            //    TimeSpan ts = new TimeSpan(23, 59, 59);
            //    DateTime _todate = date.Date + ts;
            //    query = query.Where(x => x.DateCreated <= _todate);

            //}
           
            if (advertiserId != 0)
            {
                query = query.Where(x => x.AdvertisementContentFKNavigation.AdvertiserFK == advertiserId);
            }
           
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<AdvertiseContractViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public AdvertiseContractViewModel GetById(int id)
        {
            return Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(_advertiseContractRepository.FindById(id, x => x.AdvertisementContentFKNavigation, x => x.AdvertisementContentFKNavigation.AdvertiserFKNavigation, x=>x.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }



        public void Update(AdvertiseContractViewModel advertiseContractViewModel)
        {
            var temp = _advertiseContractRepository.FindById(advertiseContractViewModel.KeyId);
            if (temp != null)
            {
                temp.ContractValue = advertiseContractViewModel.ContractValue;
                temp.DateStart = advertiseContractViewModel.DateStart;
                temp.DateFinish = advertiseContractViewModel.DateFinish;
                temp.Paid = advertiseContractViewModel.Paid;
                temp.Status = advertiseContractViewModel.Status;
            }
        }

        public void UpdateStatus(int id, int status, string note)
        {
            var temp = _advertiseContractRepository.FindById(id);
            if (temp != null)
            {
                temp.Status = (ContractStatus)status;
                temp.Note = note;
            }

        }
    }
}
