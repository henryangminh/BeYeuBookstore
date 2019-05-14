using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class BooksOutDetailViewModel
    {
        public int KeyId { get; set; }
        public int BooksOutFK { get; set; }
        public int BookFK { get; set; }
        public int Qty { get; set; }

        public virtual BooksOutViewModel BooksOutFKNavigation { get; set; }
        public virtual BookViewModel BookFKNavigation { get; set; }
    }
}
