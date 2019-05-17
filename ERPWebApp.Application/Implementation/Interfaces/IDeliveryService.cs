using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Utilities.DTOs;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Application.Interfaces
{
    public interface IDeliveryService : IDisposable
    {
        DeliveryViewModel Add(DeliveryViewModel DeliveryViewModel);

        void Update(DeliveryViewModel DeliveryViewModel);

        void Delete(int id);

        List<DeliveryViewModel> GetAll();

        PagedResult<DeliveryViewModel> GetAllPaging(int merchantId, int status, string fromdate, string todate, string keyword, int page, int pageSize);

        List<DeliveryViewModel> GetAll(int id);

        void UpdateStatus(DeliveryViewModel DeliveryViewModel);


        DeliveryViewModel GetById(int id);

        bool Save();

        DeliveryViewModel GetByDeliveryByInvoiceAndMerchant(int InvoiceId, int MerchantId);
    }
}
