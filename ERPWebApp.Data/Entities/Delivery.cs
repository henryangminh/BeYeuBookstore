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

        public Delivery(int keyId, int invoiceFK, int deliveryStatus, int merchantFK, decimal totalPrice,decimal shipPrice,string note, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            InvoiceFK = invoiceFK;
            DeliveryStatus = deliveryStatus;
            MerchantFK = merchantFK;
            OrderPrice = totalPrice;
            ShipPrice = shipPrice;
            Note = note;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int InvoiceFK { get; set; }
        public int DeliveryStatus { get; set; }
        public int MerchantFK { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal ShipPrice { get; set; }
        public string Note { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual Invoice InvoiceFKNavigation { get; set; }

        public virtual Merchant MerchantFKNavigation { get; set; }
    }
}
