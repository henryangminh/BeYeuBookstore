using BeYeuBookstore.Data.eEnum;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Customer:DomainEntity<int>,IDateTracking
    {
        public Guid UserFK { get; set; }
        public int BeYeuXu { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [ForeignKey("UserFK")]
        public virtual User UserBy { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
