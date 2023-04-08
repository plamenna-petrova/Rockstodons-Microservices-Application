using Catalog.API.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> applicationUserEntityTypeBuilder)
        {
            applicationUserEntityTypeBuilder
                .HasMany(au => au.Claims)
                .WithOne()
                .HasForeignKey(au => au.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            applicationUserEntityTypeBuilder
                .HasMany(au => au.Logins)
                .WithOne()
                .HasForeignKey(au => au.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            applicationUserEntityTypeBuilder
                .HasMany(au => au.Roles)
                .WithOne()
                .HasForeignKey(au => au.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
