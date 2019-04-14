using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels.Boq
{
    public class SO_NotificationGeneralDetailViewModel
    {
        public int KeyId { get; set; }
        public int? NotificationFk { get; set; }
        public Guid? UserFk { get; set; }
        public int Status { get; set; }
    }
}
