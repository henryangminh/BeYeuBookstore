using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class AdvertisementContentRepository : EFRepository<AdvertisementContent, int>, IAdvertisementContentRepository
    {
        ERPDbContext _context;

        public AdvertisementContentRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
