
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels.Boq
{
    public class SO_NotificationGeneralViewModel
    {
        public int KeyId { get; set; }
        public string NotificationContent { get; set; }
        public int? Referent { get; set; }

        public List<SO_NotificationGeneralDetailViewModel> SoNotificationGeneralDetail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
