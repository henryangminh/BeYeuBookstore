using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BookCategoryRepository : EFRepository<BookCategory, int>, IBookCategoryRepository
    {
        ERPDbContext _context;

        public BookCategoryRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
