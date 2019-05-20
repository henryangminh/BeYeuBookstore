using BeYeuBookstore.Data.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BeYeuBookstore.Data.EF.Repositories
{
    public class AppUserRolesRepository : IAppUserRolesRepository
    {
        public void Add(IdentityUserRole<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public IdentityUserRole<Guid> AddReturn(IdentityUserRole<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IdentityUserRole<Guid>> FindAll(params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IdentityUserRole<Guid>> FindAll(Expression<Func<IdentityUserRole<Guid>, bool>> predicate, params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IdentityUserRole<Guid>> FindAllNoTracking(params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IdentityUserRole<Guid>> FindAllNoTracking(Expression<Func<IdentityUserRole<Guid>, bool>> predicate, params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IdentityUserRole<Guid> FindById(Guid id, params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IdentityUserRole<Guid> FindSingle(Expression<Func<IdentityUserRole<Guid>, bool>> predicate, params Expression<Func<IdentityUserRole<Guid>, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void Remove(IdentityUserRole<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiple(List<IdentityUserRole<Guid>> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IdentityUserRole<Guid> entity)
        {
            throw new NotImplementedException();
        }
    }
}
