using CatalogueService.DAL.Data;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;
using CatalogueService.DAL.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogueService.DAL.Repositories
{
    public class EstateRepository : GenericRepository<Estate>, IEstateRepository
    {
        private readonly EstateDbContext _context;

        public EstateRepository(EstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Estate>> GetAllAsync<TKey>(
            Expression<Func<Estate, TKey>> sortParameter,
            Expression<Func<Estate, bool>> predicate,
            bool isDescending = false,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var entities = _context.Set<Estate>().AsNoTracking().Where(predicate);

            var sortedEntities = isDescending
                ? entities.OrderByDescending(sortParameter)
                : entities.OrderBy(sortParameter);

            var paginatedEntities = await sortedEntities.Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = entities.Count();

            return new PagedResult<Estate>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Results = paginatedEntities,
            };
        }
    }
}
