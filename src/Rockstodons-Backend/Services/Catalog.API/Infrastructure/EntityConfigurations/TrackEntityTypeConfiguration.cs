using Catalog.API.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class TrackEntityTypeConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> trackEntityTypeBuilder)
        {
            trackEntityTypeBuilder
                .HasOne(t => t.Album)
                .WithMany()
                .HasForeignKey(t => t.AlbumId);
        }
    }
}
