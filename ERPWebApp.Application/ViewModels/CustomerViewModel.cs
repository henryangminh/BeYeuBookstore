using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class CustomerViewModel
    {
        public int KeyId { get; set; }
        public Guid UserFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [ForeignKey("UserFK")]
        public virtual UserViewModel UserBy { get; set; }
    }
}
