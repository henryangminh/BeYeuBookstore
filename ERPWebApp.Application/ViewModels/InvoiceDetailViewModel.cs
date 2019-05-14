using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class InvoiceDetailViewModel
    {
        public int KeyId { get; set; }
        public int InvoiceFK { get; set; }
        public int BookFK { get; set; }
        public int MerchantFK { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual BookViewModel BookFKNavigation { get; set; }
        public virtual InvoiceViewModel InvoiceFKNavigation { get; set; }
    }
}
