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
            return Mapper.Map<AdvertiseContract, AdvertiseContractViewModel>(_advertiseContractRepository.FindById(id));
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
    }
}
