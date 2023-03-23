using Catalog.API.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class AlbumTypeEntityTypeConfiguration : IEntityTypeConfiguration<AlbumType>
    {
        public void Configure(EntityTypeBuilder<AlbumType> albumTypeEntityTypeBuilder)
        {
            albumTypeEntityTypeBuilder.ToTable("AlbumTypes");

            albumTypeEntityTypeBuilder.HasKey(at => at.Id);

            albumTypeEntityTypeBuilder.Property(at => at.Id)
                .UseHiLo("catalog_album_type_hilo")
                .IsRequired();

            albumTypeEntityTypeBuilder.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
