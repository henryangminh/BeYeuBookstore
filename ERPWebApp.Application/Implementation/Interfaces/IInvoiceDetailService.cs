using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IInvoiceDetailService : IDisposable
    {
        InvoiceDetailViewModel Add(InvoiceDetailViewModel InvoiceDetailViewModel);

        void Update(InvoiceDetailViewModel InvoiceDetailViewModel);

        void Delete(int id);

        List<InvoiceDetailViewModel> GetAll();

        PagedResult<InvoiceDetailViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<InvoiceDetailViewModel> GetAllByInvoiceIdAndMerchantId(int invoiceId, int merchantId);

        List<InvoiceDetailViewModel> GetAll(int id);

        InvoiceDetailViewModel GetById(int id);

        List<InvoiceDetailViewModel> GetAllByInvoiceId(int invoiceId);

        bool Save();
    }
}
