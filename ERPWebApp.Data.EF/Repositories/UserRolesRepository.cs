using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories

{
    public class UserRolesRepository : IUserRolesRepository
    {
        ERPDbContext _context;

        public UserRolesRepository(ERPDbContext context)
        {
            _context = context;
        }

        public void Remove(Guid userid, Guid roleid)
        {
            var item = _context.UserRoles.FirstOrDefault(p => p.RoleId == roleid && p.UserId == userid);
            if (item != null)
                _context.UserRoles.Remove(item);
        }

        public void RemoveMultiple(List<Guid> roles, Guid userid)
        {
            foreach (var item in roles)
                Remove(userid, item);
        }

    }
}
