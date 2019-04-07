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

        public List<InvoiceViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<InvoiceViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<InvoiceViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public InvoiceViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceViewModel InvoiceViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
