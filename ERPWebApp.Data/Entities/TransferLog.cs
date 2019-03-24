using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class TransferLog : DomainEntity<int>
    {
        int MerchantFK { get; set; }
    }
}
