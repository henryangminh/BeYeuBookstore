using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class BooksInDetail : DomainEntity<int>
    {
        public BooksInDetail() { }
        public BooksInDetail(int keyId, int booksInFK, int bookFK, int qty, decimal price)
        {
            KeyId = keyId;
            BooksInFK = booksInFK;
            BookFK = bookFK;
            Qty = qty;
            Price = price;
        }
        public int BooksInFK { get; set; }
        public int BookFK { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public virtual BooksIn BooksInFKNavigation { get; set; }
        public virtual Book BookFKNavigation { get; set; }
    }
}
