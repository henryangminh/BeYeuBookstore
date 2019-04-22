using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.Entities
{
    public class BookCategory: DomainEntity<int>
    {
        public BookCategory() { }
        public BookCategory(int keyId, Status status, string bookCategoryName)
        {
            KeyId = keyId;
            Status = status;
            BookCategoryName = bookCategoryName;
        }

        public string BookCategoryName { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
