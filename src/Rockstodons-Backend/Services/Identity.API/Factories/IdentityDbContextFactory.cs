using Identity.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Identity.API.Factories
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            dbContextOptionsBuilder.UseSqlServer(
                configurationBuilder["ConnectionString"], 
                sqlServerOptionsAction: o => o.MigrationsAssembly("Identity.API")
            );

            return new IdentityDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
