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
        private IRepository<Advertiser, int> _advertiserRepository;
        private IUnitOfWork _unitOfWork;
        public AdvertiseContractService(IRepository<Advertiser, int> advertiserRepository, IRepository<AdvertiseContract, int> advertiseContractRepository, IUnitOfWork unitOfWork)
        {
            _advertiserRepository = advertiserRepository;
            _advertiseContractRepository = advertiseContractRepository;
            _unitOfWork = unitOfWork;

        }
        public AdvertiseContractViewModel Add(AdvertiseContractViewModel advertiseContractViewModel)
        {
            var AdvertiseContract = Mapper.Map<AdvertiseContractViewModel, AdvertiseContract>(advertiseContractViewModel);
            var a = _advertiseContractRepository.AddReturn(AdvertiseContract);
            advertiseContractViewModel.KeyId = a.KeyId; 
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
            var query = _advertiseContractRepository.FindAll(x => x.AdvertisementContentFKNavigation, x => x.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation);
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
            var query = _advertiseContractRepository.FindAll(x=>(x.Status==ContractStatus.Requesting || x.Status == ContractStatus.AccountingCensored||x.Status == ContractStatus.DepositePaid),x=>x.AdvertisementContentFKNavigation);
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
            var query = _advertiseContractRepository.FindAll(x=>(x.Status != ContractStatus.Unqualified && x.DateFinish >= DateTime.Now && x.AdvertisementContentFKNavigation.AdvertisementPositionFK==id));
            var data = new List<AdvertiseContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<AdvertiseContractViewModel> GetAllContractByPositionId(int id)
        {
            var query = _advertiseContractRepository.FindAll(x=>(x.Status != ContractStatus.Unqualified && x.AdvertisementContentFKNavigation.AdvertisementPositionFK==id));
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
           
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.Where(x => (x.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName.ToUpper().Contains(keysearch) || x.AdvertisementContentFKNavigation.Title.ToUpper().Contains(keysearch)));

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
            query = query.OrderByDescending(x => x.KeyId);
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

        public PagedResult<AdStatisticViewModel> GetAllStatisticPaging(string fromdate, string todate, int advertiserId, int page, int pageSize)
        {
            var advertiserQuery = _advertiserRepository.FindAll();
           
            var data = new List<AdStatisticViewModel>();
            if (advertiserQuery != null)
            {
                if (advertiserId != 0)
                {
                    advertiserQuery = advertiserQuery.Where(x => x.KeyId == advertiserId);
                }
                foreach (var item in advertiserQuery)
                {
                    var query = _advertiseContractRepository.FindAll(x=>x.AdvertisementContentFKNavigation,x=>x.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation);
                    query = query.Where(x => x.AdvertisementContentFKNavigation.AdvertiserFK == item.KeyId);
                  
                    if (query.Count()!=0)
                    {
                        decimal totalContractValue = 0;
                        foreach (var _i in query)
                        {
                            if (_i.Status == ContractStatus.Requesting || _i.Status == ContractStatus.Unqualified)
                            {
                                totalContractValue = totalContractValue + _i.AdvertisementContentFKNavigation.AdvertisementPositionFKNavigation.AdvertisePrice;
                            }
                            if (_i.Status == ContractStatus.AccountingCensored || _i.Status == ContractStatus.Success)
                            {
                                totalContractValue = totalContractValue + _i.ContractValue;
                            }
                        }
                        if (!string.IsNullOrEmpty(fromdate))
                        {
                            var date = DateTime.Parse(fromdate);
                            TimeSpan ts = new TimeSpan(0, 0, 0);
                            DateTime _fromdate = date.Date + ts;
                            query = query.Where(x => x.DateStart >= _fromdate);
                          }
                        if (!string.IsNullOrEmpty(todate))
                        {
                            var date = DateTime.Parse(todate);
                            TimeSpan ts = new TimeSpan(23, 59, 59);
                            DateTime _todate = date.Date + ts;
                            query = query.Where(x => x.DateStart <= _todate);

                        }
                        var _data = new AdStatisticViewModel();
                        _data.Advertiser = item.BrandName;
                        _data.NoOfContract = query.Count();
                        _data.NoOfSuccessContract = query.Where(x => x.Status == ContractStatus.Success).Count();
                        _data.TotalContractValue = totalContractValue;
                        decimal totalPeriod = 0;
                        foreach (var _i in query)
                        {
                            if (_i.Status == ContractStatus.DepositePaid || (_i.Status == ContractStatus.Unqualified && _i.AdvertisementContentFKNavigation.CensorStatus == CensorStatus.ContentCensored))
                            {
                                totalPeriod = totalPeriod + _i.Deposite;
                            }
                            if (_i.Status == ContractStatus.AccountingCensored || _i.Status == ContractStatus.Success)
                            {
                                totalPeriod = totalPeriod + _i.ContractValue;
                            }
                        }
                        _data.TotalContractValuePeriod = totalPeriod;
                        data.Add(_data);
                    }
                }
            }



            //int totalRow = query.Count();

            //query = query.Skip((page - 1) * pageSize).Take(pageSize);

            //foreach (var item in query)
            //{
            //    var _data = new AdStatisticViewModel();
            //    _data.Advertiser = item.AdvertisementContentFKNavigation.AdvertiserFKNavigation.BrandName;
            //    _data.MonthStatistic = frommonth;
            //    var _query = _advertiseContractRepository.FindAll(x=>x.AdvertisementContentFKNavigation.AdvertiserFK);

            //    _data.NoOfContract = query.Count();
            //    data.Add(_data);
            //}

            var paginationSet = new PagedResult<AdStatisticViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = data.Count(),
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
