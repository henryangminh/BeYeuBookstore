using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BookRepository : EFRepository<Book, int>, IBookRepository
    {
        ERPDbContext _context;

        public BookRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
