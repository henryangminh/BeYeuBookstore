using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class BooksInViewModel
    {
        public int KeyId { get; set; }
        public int MerchantFK { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual MerchantViewModel MerchantFKNavigation { get; set; }
    }
}
