using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.Factories
{
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            var configurationStoreOptions = new ConfigurationStoreOptions();

            optionsBuilder.UseSqlServer(
                configurationBuilder["ConnectionString"],
                sqlServerOptionsAction: o => o.MigrationsAssembly("Identity.API")
            );

            return new ConfigurationDbContext(
                optionsBuilder.Options, configurationStoreOptions
            );
        }
    }
}
