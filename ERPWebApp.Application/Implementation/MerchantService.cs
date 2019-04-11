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
            var query = _merchantRepository.FindAll();
            var data = new List<MerchantViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Merchant, MerchantViewModel>(item);
                data.Add(_data);
            }
            return data;
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
                temp.MerchantName = MerchantViewModel.MerchantName;
            }
        }
    }
}

