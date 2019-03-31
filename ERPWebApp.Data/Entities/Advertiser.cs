using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Advertiser : DomainEntity<int>,IDateTracking
    {
        public Advertiser() { }

        public Advertiser(int keyId, Guid userFK, string brandName, string urlToBrand, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            UserFK = userFK;
            BrandName = brandName;
            UrlToBrand = urlToBrand;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid UserFK { get; set; }
        public string BrandName { get; set; }
        public string UrlToBrand { get; set; }
        public Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User UserBy { get; set; }
        //
        public virtual ICollection<AdvertisementContent> AdvertisementContents { get; set; }
    }
}
