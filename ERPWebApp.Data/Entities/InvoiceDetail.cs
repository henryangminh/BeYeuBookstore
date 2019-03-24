using ERPWebApp.Data.Interfaces;
using ERPWebApp.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.Entities
{
    public class InvoiceDetail : DomainEntity<int>, IDateTracking
    {
        int InvoiceFK { get; set; }
        int BookFK { get; set; }
        int Qty { get; set; }
        double UnitPrice { get; set; }
        double SubTotal { get; set; }
        public DateTime? DateCreated { get ; set ; }
        public DateTime? DateModified { get; set; }

        
    }
}
