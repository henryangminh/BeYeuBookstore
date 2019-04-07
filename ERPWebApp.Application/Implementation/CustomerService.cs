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

        public List<CustomerViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerViewModel CustomerViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
