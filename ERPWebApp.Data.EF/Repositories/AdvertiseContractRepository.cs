using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class AdvertiseContractRepository : EFRepository<AdvertiseContract, int>, IAdvertiseContractRepository
    {
        ERPDbContext _context;

        public AdvertiseContractRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
