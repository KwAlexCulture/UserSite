using BOL.Interfaces;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
      where TEntity : IEntity
    {
        protected readonly EntitiesDbContext _db;

        public BaseRepository(EntitiesDbContext db)
        {
            _db = db;
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            var keyProperty = GetEntityKey(entity.GetType());
            if ((Guid)keyProperty.GetValue(entity) == Guid.Empty)
            {
                keyProperty.SetValue(entity, Guid.NewGuid());
                if (typeof(IAuditableEntity).IsAssignableFrom(typeof(TEntity)))
                {
                    // ((IAuditableEntity)entity).CreatedBy = (Guid)userId;
                    ((IAuditableEntity)entity).CreatedOn = DateTime.Now;
                }
                _db.Entry(entity).State = EntityState.Added;
            }
            else
            {
                _db.Entry(entity).State = EntityState.Modified;
            }

            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(TEntity)))
            {
                // ((IAuditableEntity)entity).ModifiedBy = (Guid)userId;
                ((IAuditableEntity)entity).ModifiedOn = DateTime.Now;
            }

            await SaveDbChanges();

            return (TEntity)await _db.Set(entity.GetType()).FindAsync(keyProperty.GetValue(entity));
        } //, Guid? userId

        public virtual async Task<TEntity> FindAsync(Guid entityId)
        {
            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(TEntity)))
            {
                return await Task.Run(() =>
                {
                    var param = Expression.Parameter(typeof(TEntity));
                    var keyProperty = GetEntityKey(typeof(TEntity));
                    var expression = Expression.Equal(Expression.Property(param, keyProperty), Expression.Constant(entityId));
                    var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, param);
                    return List().SingleOrDefault(lambda);  //.Include("Creator").Include("Modifier")
                });
            }
            else
            {
                return (TEntity)await _db.Set(typeof(TEntity)).FindAsync(entityId);
            }

        }

        public virtual IQueryable<TEntity> List()
        {
            if (typeof(IAuditableEntity).IsAssignableFrom(typeof(TEntity)))
            {
                return (IQueryable<TEntity>)_db.Set(typeof(TEntity)); //.Include("Creator").Include("Modifier");
            }
            else
            {
                return (IQueryable<TEntity>)_db.Set(typeof(TEntity));
            }
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return List().Where(predicate);
        }

        public virtual async Task<bool> Delete(Guid entityId)
        {
            var entity = await FindAsync(entityId);
            _db.Set(typeof(TEntity)).Attach(entity);
            _db.Set(typeof(TEntity)).Remove(entity);
            await SaveDbChanges();
            return true;
        }

        private async Task SaveDbChanges()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PropertyInfo GetEntityKey(Type type)
        {
            var keyProperty = type.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))
                .SingleOrDefault();

            if (keyProperty == null)
            {
                throw new Exception("Hey did you forget to add a key ??");
            }

            return keyProperty;
        }

    }
}
