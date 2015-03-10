using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlingMe.Domain.Contracts
{
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            string orderByField = null, bool ascending = true, string includeProperties = "");
        TEntity GetByID(int entityID);
        void Insert(TEntity entity);
        void Delete(int entityID);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
