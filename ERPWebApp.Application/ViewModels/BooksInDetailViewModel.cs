using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.ViewModels
{
    public class BooksInDetailViewModel
    {
        public int KeyId { get; set; }
        public int BooksInFK { get; set; }
        public int BookFK { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public virtual BooksInViewModel BooksInFKNavigation { get; set; }
        public virtual BookViewModel BookFKNavigation { get; set; }
    }
}
