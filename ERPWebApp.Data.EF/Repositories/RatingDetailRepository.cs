using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class RatingDetailRepository : EFRepository<RatingDetail, int>, IRatingDetailRepository
    {
        ERPDbContext _context;

        public RatingDetailRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
