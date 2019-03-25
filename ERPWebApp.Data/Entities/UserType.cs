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

        public UserType(int keyid, string userTypeTableName, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyid;
            UserTypeTableName = userTypeTableName;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string UserTypeTableName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
