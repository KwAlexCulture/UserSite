using BOL.Interfaces;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BaseService<TEntity>
          where TEntity : IEntity
    {
        protected readonly IBaseRepository<TEntity> _repo;

        public BaseService(IBaseRepository<TEntity> repo)
        {
            _repo = repo;
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity) //, Guid? userId
        {
            return await _repo.SaveAsync(entity);  //, userId
        }

        public virtual async Task<TEntity> FindAsync(Guid entityId)
        {
            return await _repo.FindAsync(entityId);
        }

        public virtual IQueryable<TEntity> List()
        {
            return _repo.List();
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _repo.Where(predicate);
        }

        public virtual async Task<bool> Delete(Guid entityId)
        {
            return await _repo.Delete(entityId);
        }

    }
}
