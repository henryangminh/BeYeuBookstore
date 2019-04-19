using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Invoice : DomainEntity<int>, IDateTracking
    {
        public Invoice() { }

        public Invoice(int keyId, int customerFK, decimal totalPrice, string deliAddress, string deliContactName, string deliContactHotline, DateTime? dateCreated, DateTime? dateModified)
        {                                                             
            KeyId = keyId;                                           
            CustomerFK = customerFK;
            TotalPrice = totalPrice;
            DeliAddress = deliAddress;
            DeliContactName = deliContactName;
            DeliContactHotline = deliContactHotline;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }
        //Khóa ngoại Customer
        public int CustomerFK { get; set; }
        //Tổng tiền
        public decimal TotalPrice { get; set; }
        public string DeliAddress { get; set; }
        public string DeliContactName { get; set; }
        public string DeliContactHotline { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual Customer CustomerFKNavigation { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        //
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
