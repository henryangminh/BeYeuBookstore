using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class BooksOut : DomainEntity<int>, IDateTracking
    {
        public BooksOut() { }
        public BooksOut(int keyId, int merchantFK, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            MerchantFK = merchantFK;
            DateCreated = dateCreated;
            DateModified = dateModified;
            
        }
        public int MerchantFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get ; set; }

        public virtual Merchant MerchantFKNavigation { get; set; }
        public virtual ICollection<BooksOutDetail> BooksOutDetails { get; set; }
    }
}
