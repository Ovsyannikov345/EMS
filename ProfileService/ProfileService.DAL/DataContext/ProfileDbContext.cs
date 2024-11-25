using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.Models;

namespace ProfileService.DAL.DataContext
{
    public class ProfileDbContext : DbContext
    {
        static ProfileDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public ProfileDbContext(DbContextOptions options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<UserProfile> Profiles { get; set; }

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
