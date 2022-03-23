using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BOL.Interfaces;

namespace DAL.Interface
{
    public interface IBaseRepository<TEntity>
          where TEntity : IEntity
    {
        Task<TEntity> SaveAsync(TEntity entity); //, Guid? userId
        Task<TEntity> FindAsync(Guid entityId);
        IQueryable<TEntity> List();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Delete(Guid entityId);
    }
}

