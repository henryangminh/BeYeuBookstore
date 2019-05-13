using AutoMapper;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Application.Implementation
{
    public class MerchantContractService : IMerchantContractService
    {
        private IRepository<MerchantContract, int> _merchantContractRepository;
        private IUnitOfWork _unitOfWork;
        public MerchantContractService(IRepository<MerchantContract, int> merchantContractRepository, IUnitOfWork unitOfWork)
        {
            _merchantContractRepository = merchantContractRepository;
            _unitOfWork = unitOfWork;
        }
        public MerchantContractViewModel Add(MerchantContractViewModel MerchantContractViewModel)
        {
            var merchantContract = Mapper.Map<MerchantContractViewModel, MerchantContract>(MerchantContractViewModel);
            _merchantContractRepository.Add(merchantContract);
            _unitOfWork.Commit();
            return MerchantContractViewModel;
        }

        public void Delete(int id)
        {
            _merchantContractRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<MerchantContractViewModel> GetAll()
        {
            var query = _merchantContractRepository.FindAll();
            var data = new List<MerchantContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<MerchantContract, MerchantContractViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<MerchantContractViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<MerchantContractViewModel> GetAllPaging(int? merchantId, string fromdate, string todate, string keyword, int page, int pageSize)
        {

            var query = _merchantContractRepository.FindAll(p => p.MerchantFKNavigation, p => p.MerchantFKNavigation.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();
                query = query.OrderBy(x => x.KeyId).Where(x => x.MerchantFKNavigation.UserBy.FullName.ToUpper().Contains(keysearch));

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
            if (merchantId!=0)
            {
                query = query.Where(x => x.MerchantFK == merchantId);

            }
           
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<MerchantContractViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<MerchantContract, MerchantContractViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<MerchantContractViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public MerchantContractViewModel GetById(int id)
        {
            return Mapper.Map<MerchantContract, MerchantContractViewModel>(_merchantContractRepository.FindById(id));
        }
    

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

    public void Update(MerchantContractViewModel MerchantContractViewModel)
        {
            var temp = _merchantContractRepository.FindById(MerchantContractViewModel.KeyId);
            if (temp != null)
            {
                temp.MerchantFK = MerchantContractViewModel.MerchantFK;
                temp.ContractLink = MerchantContractViewModel.ContractLink;
            }
        }
    }
}
