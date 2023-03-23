using Catalog.API.Data.Data.Common.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure
{
    internal static class EntityIndexesConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
           var deletableEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));

            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }
    }
}
