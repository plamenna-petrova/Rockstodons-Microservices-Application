using Catalog.API.DataModels;
using Catalog.API.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Album> Albums { get; set; } = default!;

        public DbSet<Genre> Genres { get; set; } = default!;

        public DbSet<AlbumType> AlbumTypes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GenreEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AlbumEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AlbumEntityTypeConfiguration());
        }
    }
}
