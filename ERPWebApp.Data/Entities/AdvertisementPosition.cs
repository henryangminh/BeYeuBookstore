using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class AdvertisementPosition:DomainEntity<int>, IDateTracking
    {
        public AdvertisementPosition() { }

        public AdvertisementPosition(int keyId, string pageUrl, string idOfPosition, string title, string img,int advertisePrice, int height, int width ,Status status, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            PageUrl = pageUrl;
            IdOfPosition = idOfPosition;
            Title = title;
            Img = img;
            AdvertisePrice = advertisePrice;
            Height = height;
            Width = width;
            Status = status;
        }

        public string PageUrl { get; set; }
        public string IdOfPosition { get; set; }
        /// <summary>
        /// Tiêu đề vị trí
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Ảnh minh họa
        /// </summary>
        public string Img { get; set; }
        public int AdvertisePrice { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual ICollection<AdvertisementContent> AdvertisementContents { get; set; }
        
    }
}
