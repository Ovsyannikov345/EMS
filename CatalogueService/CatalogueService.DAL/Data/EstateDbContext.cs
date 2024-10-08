using CatalogueService.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogueService.DAL.Data
{
    public class EstateDbContext(DbContextOptions<EstateDbContext> options) : DbContext(options)
    {
        public DbSet<Estate> Estates { get; set; }
    }
}
