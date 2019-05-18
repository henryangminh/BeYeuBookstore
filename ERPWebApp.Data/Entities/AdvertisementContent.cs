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

        public AdvertisementContent(int keyId, int advertiserFK, int? advertisementPositionFK, string imageLink, string title, string description, string urlToAdvertisement, CensorStatus censorStatus, int? censorFK ,string note,DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            AdvertiserFK = advertiserFK;
            AdvertisementPositionFK = advertisementPositionFK;
            ImageLink = imageLink;
            Title = title;
            Description = description;
            UrlToAdvertisement = urlToAdvertisement;
            CensorStatus = censorStatus;
            CensorFK = censorFK;
            Note=note;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }
        // Khóa ngoại đến bảng vị trí Post Quảng cáo
        public int? AdvertisementPositionFK { get; set; }
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

        //Kiểm duyệt chưa?
        public CensorStatus CensorStatus { get; set; }
        /// <summary>
        /// Người duyệt bài
        /// </summary>
        public int? CensorFK { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    
        public virtual Advertiser AdvertiserFKNavigation { get; set; }

        public virtual AdvertisementPosition AdvertisementPositionFKNavigation { get; set; }

        public virtual AdvertiseContract AdvertiseContract { get; set; }

        public virtual WebMaster WebMasterCensorFKNavigation { get; set; }
    }
}
