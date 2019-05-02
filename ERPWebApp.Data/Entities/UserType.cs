using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class UserType:DomainEntity<int>,IDateTracking
    {
        public UserType() { }

        public UserType(int keyid, string userTypeName, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyid;
            UserTypeName = userTypeName;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string UserTypeName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
