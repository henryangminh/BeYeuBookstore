using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeYeuBookstore.Data.Entities
{
    [Table("User")]
    public partial class User:IdentityUser<Guid>,IDateTracking,ISwitchable
    {
        public User()
        {
            ArAccountsReceivableAdjustments = new HashSet<ArAccountsReceivableAdjustments>();
            
        }
        public User(Guid id, string fullName, string userName,
            string email, string phoneNumber, string avatar, Status status)
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            Status = status;
        }

        public User(string fullName, string avatar, string address, DateTime? dateCreated, DateTime? dateModified, Status status, Guid? lastupdatedFk)
        {
            FullName = fullName;
            Avatar = avatar;
            Address = address;
            DateCreated = dateCreated;
            DateModified = dateModified;
            Status = status;
            LastupdatedFk = lastupdatedFk;
        }

        public string FullName { get; set; }
        public string Avatar { get; set; }

        public string Address { get; set; }

      
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public int UserTypeFK { get; set; }
        public Status Status { get; set; }

        public Guid? LastupdatedFk { get; set; }
        
        public virtual ICollection<ArAccountsReceivableAdjustments> ArAccountsReceivableAdjustments { get; set; }
        public virtual UserType UserTypeFKNavigation { get; set; }
    }
}
