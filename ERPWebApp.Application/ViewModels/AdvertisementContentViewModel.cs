using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class AdvertisementContentViewModel
    {
        public int KeyId { get; set; }
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

        public virtual AdvertiserViewModel AdvertiserFKNavigation { get; set; }

        public virtual AdvertisementPositionViewModel AdvertisementPositionFKNavigation { get; set; }

        public virtual AdvertiseContractViewModel AdvertiseContract { get; set; }

        public virtual WebMasterViewModel WebMasterCensorFKNavigation { get; set; }
    }
}
