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
            var invoiceDetail = Mapper.Map<InvoiceDetailViewModel, InvoiceDetail>(InvoiceDetailViewModel);
            _invoiceDetailRepository.Add(invoiceDetail);
            _unitOfWork.Commit();
            return InvoiceDetailViewModel;
        }

        public void Delete(int id)
        {
            _invoiceDetailRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<InvoiceDetailViewModel> GetAll()
        {
            var query = _invoiceDetailRepository.FindAll();
            var data = new List<InvoiceDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<InvoiceDetail, InvoiceDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<InvoiceDetailViewModel> GetAllByInvoiceIdAndMerchantId(int invoiceId, int merchantId)
        {
            var query = _invoiceDetailRepository.FindAll(x=>x.BookFKNavigation);
            query = query.Where(x => (x.InvoiceFK == invoiceId && x.BookFKNavigation.MerchantFK == merchantId));
            var data = new List<InvoiceDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<InvoiceDetail, InvoiceDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<InvoiceDetailViewModel> GetAllByInvoiceId(int invoiceId)
        {
            var query = _invoiceDetailRepository.FindAll(x => x.BookFKNavigation);
            query = query.Where(x => x.InvoiceFK == invoiceId);
            var data = new List<InvoiceDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<InvoiceDetail, InvoiceDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public List<InvoiceDetailViewModel> GetAll(int id)
        {
            var query = _invoiceDetailRepository.FindAll(p => p.InvoiceFK == id, p=>p.BookFKNavigation, p=>p.InvoiceFKNavigation);
            var data = new List<InvoiceDetailViewModel>();
            foreach (var item in query)
            {
                var _data = Mapper.Map<InvoiceDetail, InvoiceDetailViewModel>(item);
                data.Add(_data);
            }
            return data;
        }

        public PagedResult<InvoiceDetailViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public InvoiceDetailViewModel GetById(int id)
        {
            return Mapper.Map<InvoiceDetail, InvoiceDetailViewModel>(_invoiceDetailRepository.FindById(id));
        }

        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(InvoiceDetailViewModel InvoiceDetailViewModel)
        {
            var temp = _invoiceDetailRepository.FindById(InvoiceDetailViewModel.KeyId);
            if (temp != null)
            {
                temp.BookFK = InvoiceDetailViewModel.BookFK;
                temp.SubTotal = InvoiceDetailViewModel.SubTotal;
                temp.InvoiceFK = InvoiceDetailViewModel.InvoiceFK;
            }
        }
    }
}
