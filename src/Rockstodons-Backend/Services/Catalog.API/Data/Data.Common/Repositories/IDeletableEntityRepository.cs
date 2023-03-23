using Catalog.API.Data.Data.Common.Models.Interfaces;

namespace Catalog.API.Data.Data.Common.Repositories
{
    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> GetAllWithDeletedRecords();

        IQueryable<TEntity> GetAllAsNoTrackingWithDeletedRecords();

        void HardDelete(TEntity entity);

        void Restore(TEntity entity);
    }
}
