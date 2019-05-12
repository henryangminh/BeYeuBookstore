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

        public CustomerViewModel GetBysId(string id)
        {
            var query = _customerRepository.FindAll(p => p.UserBy);
            var _data = new CustomerViewModel();
            foreach (var item in query)
            {
                if ((item.UserFK.ToString()) == id)
                {
                    _data = Mapper.Map<Customer, CustomerViewModel>(item);
                }
            }
            return _data;
        }

        public List<CustomerViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<CustomerViewModel> GetAllPaging(int? status, string keyword, int page, int pageSize)
        {
            var query = _customerRepository.FindAll(x=>x.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => (x.UserBy.UserName.ToUpper().Contains(keysearch)));

            }
            if (status.HasValue)
            {
                query = query.Where(x => x.UserBy.Status == (Status)status);
            }
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<CustomerViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Customer, CustomerViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<CustomerViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public CustomerViewModel GetById(int id)
        {
            return Mapper.Map<Customer, CustomerViewModel>(_customerRepository.FindById(id, x=>x.UserBy));
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
                temp.DateModified = CustomerViewModel.DateModified;

            }
        }
    }
}
