using BeYeuBookstore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeYeuBookstore.Data.IRepositories
{
   public interface IUserRolesRepository
    {
        // lấy theo user hay lấy all
        void Remove(Guid userid, Guid roleid);
        void RemoveMultiple(List<Guid> roles,Guid userid);
    }
}
