using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Data.Entities
{
    public partial class ArAccountsReceivableAdjustments : DomainEntity<int>
    {
        public ArAccountsReceivableAdjustments()
        {
            Amount = 0;
        }

        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string CustomerNo { get; set; }
        public int? TranType { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string DebitAccount { get; set; }
        public string CreditAccount { get; set; }
        public decimal Amount { get; set; }
        public Guid? AuthorizedBy { get; set; }

        public virtual User AuthorizedByNavigation { get; set; }
    }
}
