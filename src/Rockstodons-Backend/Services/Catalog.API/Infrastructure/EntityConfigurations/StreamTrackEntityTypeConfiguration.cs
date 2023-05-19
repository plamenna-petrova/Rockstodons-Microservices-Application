using Catalog.API.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class StreamTrackEntityTypeConfiguration: IEntityTypeConfiguration<StreamTrack>
    {
        public void Configure(EntityTypeBuilder<StreamTrack> streamTrackEntityTypeBuilder)
        {
            streamTrackEntityTypeBuilder
                .HasKey(st => new { st.StreamId, st.TrackId });
            streamTrackEntityTypeBuilder
                .HasOne(st => st.Stream)
                .WithMany(st => st.StreamTracks)
                .HasForeignKey(st => st.StreamId);
            streamTrackEntityTypeBuilder
                .HasOne(st => st.Track)
                .WithMany(st => st.StreamTracks)
                .HasForeignKey(st => st.TrackId);
        }
    }
}
