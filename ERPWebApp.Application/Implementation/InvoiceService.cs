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
    public class InvoiceService : IInvoiceService
    {
        private IRepository<Invoice, int> _invoiceRepository;
        private IUnitOfWork _unitOfWork;
        public InvoiceService(IRepository<Invoice, int> invoiceRepository, IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }
        public InvoiceViewModel Add(InvoiceViewModel InvoiceViewModel)
        {
            var invoice = Mapper.Map<InvoiceViewModel, Invoice>(InvoiceViewModel);
            _invoiceRepository.Add(invoice);
            _unitOfWork.Commit();
            return InvoiceViewModel;
        }


        public int GetLastest()
        {
            var invoice = _invoiceRepository.FindAll();
            invoice = invoice.OrderByDescending(x => x.KeyId);
            var _invoice = invoice.First();
            return _invoice.KeyId;
        }

        public void Delete(int id)
        {
            _invoiceRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<InvoiceViewModel> GetAll()
        {
            var query = _invoiceRepository.FindAll();
            var data = new List<InvoiceViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Invoice, InvoiceViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<InvoiceViewModel> GetAllInvoiceByCustomerId(int id)
        {
            var query = _invoiceRepository.FindAll(x => x.CustomerFK == id);
            var data = new List<InvoiceViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Invoice, InvoiceViewModel>(item);
                data.Add(_data);
            }
            return data;
        }


        public List<InvoiceViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<InvoiceViewModel> GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize)
        {
            var query = _invoiceRepository.FindAll(p => p.CustomerFKNavigation.UserBy);
            query = query.OrderBy(x => x.KeyId);
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();

                query = query.OrderBy(x => x.KeyId).Where(x => x.CustomerFKNavigation.UserBy.FullName.ToUpper().Contains(keysearch));

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
            

            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = new List<InvoiceViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<Invoice, InvoiceViewModel>(item);
                data.Add(_data);
            }

            var paginationSet = new PagedResult<InvoiceViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public InvoiceViewModel GetById(int id)
        {
            return Mapper.Map<Invoice, InvoiceViewModel>(_invoiceRepository.FindById(id, p=>p.CustomerFKNavigation.UserBy));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(InvoiceViewModel InvoiceViewModel)
        {
            var temp = _invoiceRepository.FindById(InvoiceViewModel.KeyId);
            if (temp != null)
            {
                temp.TotalPrice = InvoiceViewModel.TotalPrice;
            }
        }
    }
}
