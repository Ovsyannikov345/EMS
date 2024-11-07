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
    }
}
