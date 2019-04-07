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
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private IRepository<InvoiceDetail, int> _invoiceDetailRepository;
        private IUnitOfWork _unitOfWork;
        public InvoiceDetailService(IRepository<InvoiceDetail, int> invoiceDetailRepository, IUnitOfWork unitOfWork)
        {
            _invoiceDetailRepository = invoiceDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public InvoiceDetailViewModel Add(InvoiceDetailViewModel InvoiceDetailViewModel)
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

        public List<InvoiceDetailViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<InvoiceDetailViewModel> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<InvoiceDetailViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public InvoiceDetailViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceDetailViewModel InvoiceDetailViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
