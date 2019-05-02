using BeYeuBookstore.Data.Enums;
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
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Address { set; get; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { set; get; }
        
        public DateTime? Dob { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdDate { get; set; }


        //public int? City_FK { get; set; }
        public int UserTypeFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Gender? Gender { get; set; }
      

        public Status Status { get; set; }

        public Guid? LastupdatedFk { get; set; }
        public string LastupdatedName { get; set; }
        public string UserTypeName { get; set; }

        public List<string> Roles { get; set; }
        public virtual UserTypeViewModel UserTypeFKNavigation { get; set; }

    }
}
