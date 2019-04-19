using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class DeliveryViewModel
    {
        public int KeyId { get; set; }
        public int InvoiceFK { get; set; }
        public int DeliveryStatus { get; set; }
        public int MerchantFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual InvoiceViewModel InvoiceFKNavigation { get; set; }
    }
}
