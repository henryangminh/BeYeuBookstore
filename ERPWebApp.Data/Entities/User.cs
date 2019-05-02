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
    public partial class User : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public User()
        {
           

        }
        //public User(Guid id, string fullName, string userName,
        //    string email, string phoneNumber, string avatar, Status status)
        //{
        //    Id = id;
        //    FullName = fullName;
        //    UserName = userName;
        //    Email = email;
        //    PhoneNumber = phoneNumber;
        //    Avatar = avatar;
        //    Status = status;
        //}

        public User(Guid id, string fullName, string avatar, string address, DateTime? dateCreated, DateTime? dateModified, Status status, Guid? lastupdatedFk, string email, int userTypeFK)
        {
            Id = id;
            FullName = fullName;
            Avatar = avatar;
            Address = address;
            DateCreated = dateCreated;
            DateModified = dateModified;
            Status = status;
            LastupdatedFk = lastupdatedFk;
            Email = email;
            UserTypeFK = userTypeFK;
        }

        public string FullName { get; set; }
        public string Avatar { get; set; }

        public string Address { get; set; }
        public string IdNumber { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? IdDate { get; set; }
        public Gender? Gender { get; set; }


        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UserTypeFK { get; set; }
        //Kiểm duyệt theo nhiều trường hợp
        public Status Status { get; set; }

        public Guid? LastupdatedFk { get; set; }

        public virtual UserType UserTypeFKNavigation { get; set; }
        public virtual Customer CustomerFKNavigation { get; set; }
        public virtual Advertiser AdvertiserFKNavigation { get; set; }
        public virtual Merchant MerchantFKNavigation { get; set; }
        public virtual WebMaster WebMasterFKNavigation { get; set; }
    }
}
