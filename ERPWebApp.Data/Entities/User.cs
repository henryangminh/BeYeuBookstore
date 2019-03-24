using BeYeuBookstore.Data.eEnum;
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

        public User(Guid id,string fullName, string avatar, DateTime? dob, string idNumber,
            DateTime? idDate, string phoneNumber, string email,
            string taxIdnumber, string street, string ward, 
            string district, string city_FK, string country, int? origin_FK, 
           Gender? gender, string fax, string notes, bool? isCustomer, bool? isVendor, 
           bool? isEmployee, bool? isPersonal, Status status, Guid? lastupdatedFk, 
           string lastupdatedName)
        {
            Id = id;
            FullName = fullName;
            Avatar = avatar;
            Dob = dob;
            IdNumber = idNumber;
            IdDate = idDate;
            PhoneNumber = phoneNumber;
            Email = email;
            TaxIdnumber = taxIdnumber;
            Street = street;
            Ward = ward;
            District = district;
            City = city_FK;
            Country = country;
            Origin_FK = origin_FK;
            Gender = gender;
            Fax = fax;
            Notes = notes;
            IsCustomer = isCustomer;
            IsVendor = isVendor;
            IsEmployee = isEmployee;
            IsPersonal = isPersonal;
            Status = status;
            LastupdatedFk = lastupdatedFk;
            LastupdatedName = lastupdatedName;
        }

        public string FullName { get; set; }
        public string Avatar { get; set; }
        public DateTime? Dob { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdDate { get; set; }
       
        public string TaxIdnumber { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public int? Origin_FK { get; set; } // 
      

        public DateTime? LastLogIn { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Gender? Gender { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }
        [DefaultValue(false)]
        public bool? IsCustomer { get; set; }
        [DefaultValue(false)]
        public bool? IsVendor { get; set; }
        [DefaultValue(false)]
        public bool? IsEmployee { get; set; }
        [DefaultValue(false)]
        public bool? IsPersonal { get; set; }

        public Status Status { get; set; }

        public Guid? LastupdatedFk { get; set; }
        public string LastupdatedName{ get; set; }
      //  public virtual GnCity CityFkNavigation { get; set; }
       
        
        public virtual ICollection<ArAccountsReceivableAdjustments> ArAccountsReceivableAdjustments { get; set; }
     
     
        }
}
