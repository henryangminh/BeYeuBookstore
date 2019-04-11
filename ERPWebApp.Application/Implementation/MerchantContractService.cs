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

        public PagedResult<MerchantContractViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
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
