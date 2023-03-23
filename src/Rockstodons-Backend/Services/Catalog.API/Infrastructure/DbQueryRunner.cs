using Catalog.API.Data.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure
{
    public class DbQueryRunner : IDbQueryRunner
    {
        public DbQueryRunner(CatalogDbContext catalogDbContext)
        {
            DbContext = catalogDbContext ?? throw new ArgumentNullException(nameof(catalogDbContext));
        }

        public CatalogDbContext DbContext { get; set; }

        public Task RunQueryAsync(string query, params object[] parameters)
            => DbContext.Database.ExecuteSqlRawAsync(query, parameters);

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
