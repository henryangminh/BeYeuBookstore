using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class AdvertiserRepository : EFRepository<Advertiser,int>, IAdvertiserRepository
    {
        ERPDbContext _context;

        public AdvertiserRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
