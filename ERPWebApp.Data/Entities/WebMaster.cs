using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class WebMaster: DomainEntity<int>, IDateTracking
    {
        public WebMaster()
        {

        }

        public WebMaster(int keyId, Guid userFK, int webMasterTypeFK, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            UserFK = userFK;
            WebMasterTypeFK = webMasterTypeFK;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid UserFK { get; set; }
        public int WebMasterTypeFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual User UserBy { get; set; }
        //
        public virtual WebMasterType WebMasterTypeFKNavigation { get; set; }

        public virtual ICollection<AdvertisementContent> AdvertisementContents { get; set; }
    }
}
