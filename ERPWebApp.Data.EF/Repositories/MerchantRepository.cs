using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class MerchantRepository : EFRepository<Merchant, int>, IMerchantRepository
    {
        ERPDbContext _context;

        public MerchantRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
