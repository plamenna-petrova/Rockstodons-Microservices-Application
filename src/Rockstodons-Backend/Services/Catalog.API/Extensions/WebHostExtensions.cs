using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Catalog.API.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost MigrateDbContext<TDbContext>(
            this IWebHost webHost,
            Action<TDbContext, IServiceProvider> seeder
        ) where TDbContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<TDbContext>>();
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            try
            {
                logger.LogInformation(
                    "Migrating database, associated with context {DbContextName}",
                    typeof(TDbContext).Name
                );

                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8)
                    });

                retry.Execute(() => InvokeSeeder(seeder, dbContext, serviceProvider));

                logger.LogInformation(
                    "Migrated database, associated with context {DbContextName}", 
                    typeof(DbContext).Name
                );
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred, while migrating the database, " +
                    "used on context {DbContextName}", typeof(TDbContext).Name);
            }

            return webHost; 
        }

        private static void InvokeSeeder<TDbContext>(
            Action<TDbContext, IServiceProvider> seeder, 
            TDbContext dbContext, 
            IServiceProvider serviceProvider
        ) 
            where TDbContext : DbContext
        {
            dbContext.Database.Migrate();
            seeder(dbContext, serviceProvider);
        }
    }
}
