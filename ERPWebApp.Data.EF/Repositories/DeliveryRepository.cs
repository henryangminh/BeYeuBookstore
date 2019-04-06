using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class DeliveryRepository : EFRepository<Delivery, int>, IDeliveryRepository
    {
        ERPDbContext _context;

        public DeliveryRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
