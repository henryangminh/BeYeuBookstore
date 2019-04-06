using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class InvoiceViewModel
    {
        public int KeyId { get; set; }
        public int CustomerFK { get; set; }
        //Tổng tiền
        public decimal TotalPrice { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual CustomerViewModel CustomerFKNavigation { get; set; }
        public virtual DeliveryViewModel DeliveryFKNavigation { get; set; }
    }
}
