using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BooksOutRepository : EFRepository<BooksOut, int>, IBooksOutRepository
    {
        ERPDbContext _context;

        public BooksOutRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
