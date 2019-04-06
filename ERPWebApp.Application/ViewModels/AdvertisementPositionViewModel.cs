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
        public int AdvertisePrice { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Status Status { get; set; }
    }
}
