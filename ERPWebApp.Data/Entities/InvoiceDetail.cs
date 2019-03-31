using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class InvoiceDetail :DomainEntity<int>, IDateTracking
    {
        public InvoiceDetail() { }

        public InvoiceDetail(int invoiceFK, int bookFK, int qty, decimal unitPrice, decimal subTotal, DateTime? dateCreated, DateTime? dateModified)
        {
            InvoiceFK = invoiceFK;
            BookFK = bookFK;
            Qty = qty;
            UnitPrice = unitPrice;
            SubTotal = subTotal;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int InvoiceFK { get; set; }
        public int BookFK { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime? DateCreated { get ; set ; }
        public DateTime? DateModified { get; set; }
        //
        public virtual Book BookFKNavigation { get; set; }
        public virtual Invoice InvoiceFKNavigation { get; set; }
    }
}
