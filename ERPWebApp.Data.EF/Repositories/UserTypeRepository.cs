using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class UserTypeRepository : EFRepository<UserType, int>, IUserTypeRepository
    {
        ERPDbContext _context;

        public UserTypeRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
