using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class CustomerRepository : EFRepository<Customer, int>, ICustomerRepository
    {
        ERPDbContext _context;

        public CustomerRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
