using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class InvoiceRepository : EFRepository<Invoice, int>, IInvoiceRepository
    {
        ERPDbContext _context;

        public InvoiceRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
