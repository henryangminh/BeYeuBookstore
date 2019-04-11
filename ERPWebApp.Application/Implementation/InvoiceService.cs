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
            return Mapper.Map<Invoice, InvoiceViewModel>(_invoiceRepository.FindById(id));
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
