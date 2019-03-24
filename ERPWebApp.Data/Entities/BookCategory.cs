using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class BookCategory: DomainEntity<int>
    {
        public BookCategory() { }
        public BookCategory(int keyId, string bookCategoryName)
        {
            KeyId = keyId;
            BookCategoryName = bookCategoryName;
        }

        public string BookCategoryName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
