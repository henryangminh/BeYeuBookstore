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

        public List<MerchantViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<MerchantViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<MerchantViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public MerchantViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(MerchantViewModel MerchantViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
