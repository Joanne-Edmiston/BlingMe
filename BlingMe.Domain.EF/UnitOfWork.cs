using BlingMe.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlingMe.Domain.EF.Repositories;

namespace BlingMe.Domain.EF
{
    public class UnitOfWork : IDisposable
    {
        private EFDbContext context = new EFDbContext();
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IEntityRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var repositoryKey = typeof(TEntity);
            if (!repositories.ContainsKey(repositoryKey))
            {
                repositories.Add(repositoryKey, new EntityRepository<TEntity>(context));
            }

            return (IEntityRepository<TEntity>)repositories[repositoryKey];
        }

        /// <summary>
        /// Save Changes to data
        /// </summary>
        /// <returns>Number of items affected</returns>
        public int Save()
        {
            return context.SaveChanges();
        }

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
