using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class AdvertisementPositionViewModel
    {
        public int KeyId { get; set; }
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
    }
}
