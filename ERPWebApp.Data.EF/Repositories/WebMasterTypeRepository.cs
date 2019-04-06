using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class WebMasterTypeRepository : EFRepository<WebMasterType, int>, IWebMasterTypeRepository
    {
        ERPDbContext _context;

        public WebMasterTypeRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
