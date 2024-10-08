using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.Models;

namespace ProfileService.DAL.DataContext
{
    public class ProfileDbContext(DbContextOptions<ProfileDbContext> options) : DbContext(options)
    {
        static ProfileDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<UserProfile> Profiles { get; set; }
    }
}
