using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Data.Entities
{
    public partial class SoNotification : DomainEntity<int>
    {
        
        public int CustomerFK { get; set; }
        public int SoId { get; set; }
        public DateTime Sodate { get; set; }
        public int? AcceptanceFk { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public int? EngineerFk { get; set; }
        public int? PaymentFk { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? CreatePaymentByFk { get; set; }
        public int? PaymentStatusFk { get; set; }
        public int? InvoiceNo { get; set; }
        public DateTime? NotificationDate { get; set; }
        public DateTime? NotificationLastUpdate { get; set; }

        
    }
}
