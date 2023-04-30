using Catalog.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class AlbumEntityTypeConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> albumEntityTypeBuilder)
        {
            albumEntityTypeBuilder.ToTable("Albums");

            albumEntityTypeBuilder.Property(a => a.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            albumEntityTypeBuilder.HasOne(a => a.Genre)
                .WithMany()
                .HasForeignKey(a => a.GenreId);

            albumEntityTypeBuilder.HasOne(a => a.AlbumType)
                .WithMany()
                .HasForeignKey(a => a.AlbumTypeId);

            albumEntityTypeBuilder.HasMany(a => a.Tracks)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId);
        }
    }
}
