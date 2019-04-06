using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class AdvertiserViewModel
    {
        public int KeyId { get; set; }
        public Guid UserFK { get; set; }
        public string BrandName { get; set; }
        public string UrlToBrand { get; set; }
        public Status Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual UserViewModel UserBy { get; set; }
    }
}
