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

        public AdvertisementContent(int keyId, int advertiserFK, string imageLink, string title, string description, string urlToAdvertisement, decimal deposite, bool? paidDeposite, bool? censorStatus,  DateTime? dateCreated, DateTime? dateModified)
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
        // Khóa ngoại đến bảng vị trí Post Quảng cáo
        public int AdvertisementPositionFK { get; set; }
        // Khóa ngoại đến người Quảng cáo
        public int AdvertiserFK { get; set; }
        //Link hình ảnh
        public string ImageLink { get; set; }
        //Tên
        public string Title { get; set; }
        //Mô tả
        public string Description { get; set; }
        //Link 16'
        public string UrlToAdvertisement { get; set; }
        //Tiền
        public decimal Deposite { get; set; }
        //Có trả tiền chưa?
        public bool? PaidDeposite { get; set; }
        //Kiểm duyệt chưa?
        public bool? CensorStatus { get; set; }
        /// <summary>
        /// Người duyệt bài
        /// </summary>
        
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    
        public virtual Advertiser AdvertiserFKNavigation { get; set; }

        public virtual AdvertisementPosition AdvertisementPositionFKNavigation { get; set; }

        public virtual AdvertiseContract AdvertiseContract { get; set; }
    }
}
