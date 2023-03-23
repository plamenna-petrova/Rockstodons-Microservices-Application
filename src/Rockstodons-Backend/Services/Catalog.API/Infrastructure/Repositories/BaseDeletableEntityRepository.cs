using Catalog.API.Data.Data.Common.Models.Interfaces;
using Catalog.API.Data.Data.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Repositories
{
    public class BaseDeletableEntityRepository<TEntity> : BaseRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public BaseDeletableEntityRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
            
        }

        public override IQueryable<TEntity> GetAll() => base.GetAll().Where(x => !x.IsDeleted);

        public override IQueryable<TEntity> GetAllAsNoTracking() => base.GetAllAsNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> GetAllWithDeletedRecords() => base.GetAll().IgnoreQueryFilters();

        public IQueryable<TEntity> GetAllAsNoTrackingWithDeletedRecords() => base.GetAllAsNoTracking().IgnoreQueryFilters();

        public void HardDelete(TEntity entity) => base.Delete(entity);

        public void Restore(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
            this.Update(entity);
        }
    }
}
