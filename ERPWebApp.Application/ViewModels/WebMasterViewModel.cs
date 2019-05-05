using BeYeuBookstore.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class WebMasterViewModel
    {
        public int KeyId { get; set; }
        public Guid UserFK { get; set; }
        public int WebMasterTypeFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual UserViewModel UserBy { get; set; }
        public virtual WebMasterTypeViewModel WebMasterTypeFKNavigation { get; set; }
       

    }
}
