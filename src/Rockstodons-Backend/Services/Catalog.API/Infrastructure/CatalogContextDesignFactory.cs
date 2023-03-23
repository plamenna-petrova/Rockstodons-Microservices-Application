using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.API.Infrastructure
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlServer("Server=LENOVOLEGION\\SQLEXPRESS;Initial Catalog=Rockstodons.CatalogDb;Integrated Security=true");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
