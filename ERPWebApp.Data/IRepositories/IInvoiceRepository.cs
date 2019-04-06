using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.IRepositories
{
    public interface IInvoiceRepository : IRepository<Invoice,int>
    {
    }
}
