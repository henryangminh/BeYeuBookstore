using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class WebMasterRepository : EFRepository<WebMaster, int>, IWebMasterRepository
    {
        ERPDbContext _context;

        public WebMasterRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
