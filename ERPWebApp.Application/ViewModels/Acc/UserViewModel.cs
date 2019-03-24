using BeYeuBookstore.Data.eEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels.System
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Roles = new List<string>();
        }
        public Guid? Id { set; get; }
        public string FullName { set; get; }
        public int? Employee_FK { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        public string UserName { set; get; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { set; get; }
        
        public DateTime? Dob { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdDate { get; set; }

        public string TaxIdnumber { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? Origin_FK { get; set; }
        //public int? City_FK { get; set; }

        public DateTime? LastLogIn { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Gender? Gender { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsVendor { get; set; }

        public bool IsEmployee { get; set; }

        public bool IsPersonal { get; set; }

        public Status Status { get; set; }

        public Guid? LastupdatedFk { get; set; }
        public string LastupdatedName { get; set; }

        public List<string> Roles { get; set; }
    }
}
