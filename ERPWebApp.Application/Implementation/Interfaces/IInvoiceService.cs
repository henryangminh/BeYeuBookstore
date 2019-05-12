using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IInvoiceService : IDisposable
    {
        InvoiceViewModel Add(InvoiceViewModel InvoiceViewModel);

        void Update(InvoiceViewModel InvoiceViewModel);

        void Delete(int id);

        List<InvoiceViewModel> GetAll();

        PagedResult<InvoiceViewModel> GetAllPaging(string fromdate, string todate, string keyword, int page, int pageSize);

        List<InvoiceViewModel> GetAll(int id);

        InvoiceViewModel GetById(int id);
        int GetLastest();
        List<InvoiceViewModel> GetAllInvoiceByCustomerId(int id);

        bool Save();
    }
}
