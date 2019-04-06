using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class AdvertisementPositionRepository : EFRepository<AdvertisementPosition, int>, IAdvertisementPositionRepository
    {
        ERPDbContext _context;

        public AdvertisementPositionRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}