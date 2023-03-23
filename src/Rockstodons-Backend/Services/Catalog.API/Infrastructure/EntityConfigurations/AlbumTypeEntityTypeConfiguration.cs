using Catalog.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class AlbumTypeEntityTypeConfiguration : IEntityTypeConfiguration<AlbumType>
    {
        public void Configure(EntityTypeBuilder<AlbumType> albumTypeEntityTypeBuilder)
        {
            albumTypeEntityTypeBuilder.ToTable("AlbumTypes");

            albumTypeEntityTypeBuilder.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
