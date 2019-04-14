using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Data.Entities
{
    public partial class SoNotificationGeneral : DomainEntity<int>, IDateTracking
    {
        public SoNotificationGeneral()
        {
            SoNotificationGeneralDetail = new HashSet<SoNotificationGeneralDetail>();
        }

        public SoNotificationGeneral(int keyId, string notificationContent, int? referent, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = KeyId;
            NotificationContent = notificationContent;
            Referent = referent;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string NotificationContent { get; set; }
        public int? Referent { get; set; }

        public ICollection<SoNotificationGeneralDetail> SoNotificationGeneralDetail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
