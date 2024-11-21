using CatalogueService.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogueService.DAL.Data
{
    public class EstateDbContext : DbContext
    {
        public DbSet<Estate> Estates { get; set; }

        public DbSet<EstateFilter> EstateFilters { get; set; }

        public EstateDbContext(DbContextOptions<EstateDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public override int SaveChanges()
        {
            AddTimestamps();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).CreatedAt = now;
                }
                else
                {
                    entity.Property(nameof(EntityBase.CreatedAt)).IsModified = false;
                }

                ((EntityBase)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
