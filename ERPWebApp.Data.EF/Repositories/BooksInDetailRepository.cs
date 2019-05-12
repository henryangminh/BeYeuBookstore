using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class BooksInDetailRepository : EFRepository<BooksInDetail, int>, IBooksInDetailRepository
    {
        ERPDbContext _context;

        public BooksInDetailRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
