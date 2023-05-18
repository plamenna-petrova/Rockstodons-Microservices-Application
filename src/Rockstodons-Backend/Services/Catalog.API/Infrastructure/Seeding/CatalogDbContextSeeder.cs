namespace Catalog.API.Infrastructure.Seeding
{
    public class CatalogDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(CatalogDbContext catalogDbContext, IServiceProvider serviceProvider)
        {
            if (catalogDbContext == null)
            {
                throw new ArgumentNullException(nameof(catalogDbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(CatalogDbContext));

            var seeders = new List<ISeeder>
            {
                new EntitiesSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(catalogDbContext, serviceProvider);
                await catalogDbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
