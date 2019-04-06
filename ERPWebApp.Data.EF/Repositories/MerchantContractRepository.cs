using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class MerchantContractRepository : EFRepository<MerchantContract, int>, IMerchantContractRepository
    {
        ERPDbContext _context;

        public MerchantContractRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
