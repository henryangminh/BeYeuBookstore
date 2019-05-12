using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class BooksOutDetail : DomainEntity<int>
    {
        public BooksOutDetail() { }
        public BooksOutDetail(int keyId, int booksOutFK, int bookFK, int qty)
        {
            KeyId = keyId;
            BooksOutFK = booksOutFK;
            BookFK = bookFK;
            Qty = qty;
           
        }
        public int BooksOutFK { get; set; }
        public int BookFK { get; set; }
        public int Qty { get; set; }

        public virtual BooksOut BooksOutFKNavigation { get; set; }
        public virtual Book BookFKNavigation { get; set; }
    }
}
