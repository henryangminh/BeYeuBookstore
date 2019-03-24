using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {
        }
        public Role(string name, string description) : base(name)
        {
            this.Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
