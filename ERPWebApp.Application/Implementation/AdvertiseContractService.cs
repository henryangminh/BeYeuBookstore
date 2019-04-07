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

        public List<AdvertiseContractViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<AdvertiseContractViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AdvertiseContractViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public AdvertiseContractViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(AdvertiseContractViewModel advertiseContractViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
