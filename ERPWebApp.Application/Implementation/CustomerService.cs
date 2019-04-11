﻿using AutoMapper;
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
    public class CustomerService : ICustomerService
    {
        private IRepository<Customer, int> _customerRepository;
        private IUnitOfWork _unitOfWork;
        public CustomerService(IRepository<Customer, int> customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }
        public CustomerViewModel Add(CustomerViewModel CustomerViewModel)
        {
            var customer = Mapper.Map<CustomerViewModel, Customer>(CustomerViewModel);
            _customerRepository.Add(customer);
            _unitOfWork.Commit();
            return CustomerViewModel;
        }

        public void Delete(int id)
        {
            _customerRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<CustomerViewModel> GetAll()
        {
            var query = _customerRepository.FindAll();
            var data = new List<CustomerViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Customer, CustomerViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<CustomerViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<CustomerViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public CustomerViewModel GetById(int id)
        {
            return Mapper.Map<Customer, CustomerViewModel>(_customerRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(CustomerViewModel CustomerViewModel)
        {
            var temp = _customerRepository.FindById(CustomerViewModel.KeyId);
            if (temp != null)
            {
                temp.Dob = CustomerViewModel.Dob;
            }
        }
    }
}
