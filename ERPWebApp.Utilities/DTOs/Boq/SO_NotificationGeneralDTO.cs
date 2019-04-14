using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.DTOs.Boq
{
    public class SO_NotificationGeneralDTO
    {
        public int KeyId { get; set; }

        public string NotificationContent { get; set; }

        public DateTime? NotificationTime { get; set; }

        public int Status { get; set; }
    }
}
