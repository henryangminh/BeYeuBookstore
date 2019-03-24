using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class AdvertisementPosition:DomainEntity<int>
    {
        public AdvertisementPosition() { }

        public AdvertisementPosition(int keyId, string pageUrl, string idOfPosition)
        {
            keyId = KeyId;
            PageUrl = pageUrl;
            IdOfPosition = idOfPosition;
        }

        public string PageUrl { get; set; }
        public string IdOfPosition { get; set; }
    }
}
