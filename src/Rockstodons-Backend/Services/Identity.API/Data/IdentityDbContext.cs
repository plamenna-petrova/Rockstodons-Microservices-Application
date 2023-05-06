using Identity.API.Models;
using Identity.API.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.API.Data
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
            
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntityTypeConfigurations(modelBuilder);

            EntityIndexesConfiguration.Configure(modelBuilder);

            var mutableEntityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            var mutableDeletableEntityTypes = mutableEntityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));

            foreach (var mutableDeletableEntityType in mutableDeletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.
                    MakeGenericMethod(mutableDeletableEntityType.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }

            var foreignKeys = mutableEntityTypes
                .SelectMany(et => et.GetForeignKeys()
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder modelBuilder)
            where T : class, IDeletableEntity
        {
            modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(IdentityDbContext)
                .GetMethod(nameof(SetIsDeletedQueryFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)!;

        private void ConfigureEntityTypeConfigurations(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
