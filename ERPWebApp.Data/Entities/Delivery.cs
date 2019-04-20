using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Delivery : DomainEntity<int>, IDateTracking
    {
        public Delivery() { }

        public Delivery(int keyId, int invoiceFK, int deliveryStatus, int merchantFK, decimal totalPrice, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            InvoiceFK = invoiceFK;
            DeliveryStatus = deliveryStatus;
            MerchantFK = merchantFK;
            TotalPrice = totalPrice;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int InvoiceFK { get; set; }
        public int DeliveryStatus { get; set; }
        public int MerchantFK { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual Invoice InvoiceFKNavigation { get; set; }

        public virtual Merchant MerchantFKNavigation { get; set; }
    }
}
