using CatalogueService.DAL.Data;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;

namespace CatalogueService.DAL.Repositories
{
    public class EstateRepository(EstateDbContext context) : GenericRepository<Estate>(context), IEstateRepository;
}
