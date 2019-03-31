using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class AdvertisementContent : DomainEntity<int>, IDateTracking
    {
        public AdvertisementContent() { }

        public AdvertisementContent(int keyId, int advertiserFK, string imageLink, string title, string description, string urlToAdvertisement, decimal deposite, bool? paidDeposite, Status? censorStatus,  DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            AdvertiserFK = advertiserFK;
            ImageLink = imageLink;
            Title = title;
            Description = description;
            UrlToAdvertisement = urlToAdvertisement;
            Deposite = deposite;
            PaidDeposite = paidDeposite;
            CensorStatus = censorStatus;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public int AdvertisementPositionFK { get; set; }
        public int AdvertiserFK { get; set; }
        public string ImageLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlToAdvertisement { get; set; }
        public decimal Deposite { get; set; }
        public bool? PaidDeposite { get; set; }
        public Status? CensorStatus { get; set; }
        /// <summary>
        /// Người duyệt bài
        /// </summary>
        
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual Advertiser AdvertiserFKNavigation { get; set; }
        //
        public virtual AdvertisementPosition AdvertisementPositionFKNavigation { get; set; }
        //
        public virtual AdvertiseContract AdvertiseContract { get; set; }
    }
}
