using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BooksInRepository : EFRepository<BooksIn, int>, IBooksInRepository
    {
        ERPDbContext _context;

        public BooksInRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
