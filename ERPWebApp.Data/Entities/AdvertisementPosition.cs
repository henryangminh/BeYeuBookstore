using BeYeuBookstore.Data.eEnum;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class AdvertisementPosition:DomainEntity<int>
    {
        public AdvertisementPosition() { }

        public AdvertisementPosition(string pageUrl, string idOfPosition, int advertisePrice, Status status)
        {
            PageUrl = pageUrl;
            IdOfPosition = idOfPosition;
            AdvertisePrice = advertisePrice;
            Status = status;
        }

        public string PageUrl { get; set; }
        public string IdOfPosition { get; set; }
        public int AdvertisePrice { get; set; }
        public Status Status { get; set; }
    }
}
