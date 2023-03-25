namespace Catalog.API.Infrastructure.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(CatalogDbContext catalogDbContext, IServiceProvider serviceProvider);
    }
}
