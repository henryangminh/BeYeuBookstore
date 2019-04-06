using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class InvoiceDetailRepository : EFRepository<InvoiceDetail,int>, IInvoiceDetailRepository
    {
        ERPDbContext _context;

        public InvoiceDetailRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
