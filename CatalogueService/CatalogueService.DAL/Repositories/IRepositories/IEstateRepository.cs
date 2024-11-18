using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Utilities.Pagination;
using System.Linq.Expressions;

namespace CatalogueService.DAL.Repositories.IRepositories
{
    public interface IEstateRepository : IGenericRepository<Estate>
    {
        Task<PagedResult<Estate>> GetAllAsync<TKey>(
            Expression<Func<Estate, TKey>> sortParameter,
            bool isDescending = false,
            Expression<Func<Estate, bool>>? predicate = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
    }
}
