using CatalogueService.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogueService.DAL.Data
{
    public class EstateDbContext : DbContext
    {
        public EstateDbContext(DbContextOptions<EstateDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<Estate> Estates { get; set; }
    }
}
