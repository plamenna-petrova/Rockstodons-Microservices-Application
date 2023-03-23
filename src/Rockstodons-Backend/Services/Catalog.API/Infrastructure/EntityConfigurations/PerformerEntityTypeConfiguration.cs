using Catalog.API.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class PerformerEntityTypeConfiguration : IEntityTypeConfiguration<Performer>
    {
        public void Configure(EntityTypeBuilder<Performer> performerEntityTypeBuilder)
        {
            performerEntityTypeBuilder.ToTable("Performers");

            performerEntityTypeBuilder
                .HasMany(p => p.Albums)
                .WithOne(a => a.Performer)
                .HasForeignKey(a => a.PerformerId);
        }
    }
}
