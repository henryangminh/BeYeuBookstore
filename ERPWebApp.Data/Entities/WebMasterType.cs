using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace BeYeuBookstore.Data.Entities
{
    public class WebMasterType:DomainEntity<int>,IDateTracking
    {
        public WebMasterType() { }

        public WebMasterType(int keyId, string webMasterTypeName, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            WebMasterTypeName = webMasterTypeName;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string WebMasterTypeName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //
        public virtual ICollection<WebMaster> WebMasters { get; set; }
    }
}