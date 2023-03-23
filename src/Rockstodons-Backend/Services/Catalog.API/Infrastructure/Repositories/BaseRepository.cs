using Catalog.API.Data.Data.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public BaseRepository(CatalogDbContext catalogDbContext)
        {
            DbContext = catalogDbContext ?? throw new ArgumentNullException(nameof(catalogContext));
            DbSet = this.DbContext.Set<TEntity>();
        }

        protected CatalogDbContext DbContext { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        public virtual IQueryable<TEntity> GetAll() => DbSet;

        public virtual IQueryable<TEntity> GetAllAsNoTracking() => DbSet.AsNoTracking();

        public virtual Task AddAsync(TEntity entity) => DbSet.AddAsync(entity).AsTask();

        public virtual void Update(TEntity entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public virtual Task<int> SaveChangesAsync() => DbContext.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext?.Dispose();
            }
        }
    }
}
