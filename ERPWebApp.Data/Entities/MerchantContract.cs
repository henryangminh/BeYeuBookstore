using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class MerchantContract:DomainEntity<int>,IDateTracking
    {
        public MerchantContract(int keyId, string contractLink, int merchantFK, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            ContractLink = contractLink;
            MerchantFK = merchantFK;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string ContractLink { get; set; }
        public int MerchantFK { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual Merchant MerchantFKNavigation { get; set; }
    }
}
