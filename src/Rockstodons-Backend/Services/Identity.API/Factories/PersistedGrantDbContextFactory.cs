using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.Factories
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            var operationalStoreOptions = new OperationalStoreOptions();

            dbContextOptionsBuilder.UseSqlServer(
                configurationBuilder["ConnectionString"], 
                sqlServerOptionsAction: o => o.MigrationsAssembly("Idedntity.API")
            );

            return new PersistedGrantDbContext(
                dbContextOptionsBuilder.Options, 
                operationalStoreOptions
            );
        }
    }
}
