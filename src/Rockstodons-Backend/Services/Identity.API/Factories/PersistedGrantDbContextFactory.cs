using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
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

            dbContextOptionsBuilder.UseSqlServer(
                configurationBuilder["ConnectionString"], 
                sqlServerOptionsAction: o => o.MigrationsAssembly("Idedntity.API")
            );

            return new PersistedGrantDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
