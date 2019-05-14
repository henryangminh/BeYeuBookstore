using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BooksOutDetailRepository : EFRepository<BooksOutDetail, int>, IBooksOutDetailRepository
    {
        ERPDbContext _context;

        public BooksOutDetailRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
