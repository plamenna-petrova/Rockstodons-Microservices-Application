using Catalog.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class GenreEntityTypeConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> genreEntityTypeBuilder)
        {
            genreEntityTypeBuilder.ToTable("Genres");

            genreEntityTypeBuilder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
