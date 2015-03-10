using BlingMe.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlingMe.Domain.EF.Helpers;

namespace BlingMe.Domain.EF.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
    {

        private EFDbContext context;
        private DbSet<TEntity> dbSet;


        public EntityRepository(EFDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            string orderByField = null, bool ascending = true, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // apply eager loading 
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderByField != null)
            {
                query = ascending ? query.OrderBy(orderByField) : query.OrderByDescending(orderByField);
            }
            else
            {
                query = query.OrderBy(e => true);
            }

            return query;
        }

        public TEntity GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
