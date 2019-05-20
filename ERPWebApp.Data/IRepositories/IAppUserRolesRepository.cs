using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Data.IRepositories
{
    public interface IAppUserRolesRepository : IRepository<IdentityUserRole<Guid>, Guid>
    {

    }
}
