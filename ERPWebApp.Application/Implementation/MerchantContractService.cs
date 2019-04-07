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

        public List<MerchantContractViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(MerchantContractViewModel MerchantContractViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
