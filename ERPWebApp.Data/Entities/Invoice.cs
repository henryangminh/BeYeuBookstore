using ERPWebApp.Data.Interfaces;
using ERPWebApp.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Data.Entities
{
    public class Invoice : DomainEntity<int>, IDateTracking
    {
        int CustomerFK { get; set; }
        double TotalPrice { get; set; }
        bool IsBYXu { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }


    }
}
