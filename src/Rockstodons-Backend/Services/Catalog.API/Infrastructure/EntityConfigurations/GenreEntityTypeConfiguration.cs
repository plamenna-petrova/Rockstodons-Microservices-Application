using Catalog.API.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class GenreEntityTypeConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> genreEntityTypeBuilder)
        {
            genreEntityTypeBuilder.ToTable("Genres");

            genreEntityTypeBuilder.HasKey(g => g.Id);

            genreEntityTypeBuilder.Property(g => g.Id)
                .UseHiLo("catalog_genre_hilo")
                .IsRequired();

            genreEntityTypeBuilder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
