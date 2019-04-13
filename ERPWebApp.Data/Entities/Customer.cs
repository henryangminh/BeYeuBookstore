using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Interfaces;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class Customer:DomainEntity<int>,IDateTracking
    {
        public Customer() { }
        public Customer(int keyId, Guid userFK, DateTime? dob, Gender gender, DateTime? dateCreated, DateTime? dateModified)
        {
            KeyId = keyId;
            UserFK = userFK;
            Dob = dob;
            Gender = gender;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }
        public Guid UserFK { get; set; }
        public DateTime? Dob { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [ForeignKey("UserFK")]
        public virtual User UserBy { get; set; }
        //
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
