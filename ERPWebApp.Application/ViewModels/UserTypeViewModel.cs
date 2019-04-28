using BeYeuBookstore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class UserTypeViewModel
    {
        public int KeyId { get; set; }
        public string UserTypeName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
