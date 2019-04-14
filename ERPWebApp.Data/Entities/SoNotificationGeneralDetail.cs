using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Data.Entities
{
    public partial class SoNotificationGeneralDetail : DomainEntity<int>
    {
        public SoNotificationGeneralDetail(int keyId, int? notificationFk, Guid? userFk, int status)
        {
            KeyId = keyId;
            NotificationFk = notificationFk;
            UserFk = userFk;
            Status = status;
        }

        public int? NotificationFk { get; set; }
        public Guid? UserFk { get; set; }
        public int Status { get; set; }

        public virtual SoNotificationGeneral NotificationFkNavigation { get; set; }
    }
}
