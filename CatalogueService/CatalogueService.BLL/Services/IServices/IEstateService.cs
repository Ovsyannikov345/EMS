using CatalogueService.BLL.Models;
using CatalogueService.BLL.Utilities.QueryParameters;
using CatalogueService.DAL.Utilities.Pagination;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateService
    {
        Task<PagedResult<EstateModel>> GetEstateListAsync(
            SortOption sortOption,
            EstateQueryFilter filter,
            Pagination pagination,
            CancellationToken cancellationToken = default);

        Task<EstateWithProfileModel> GetEstateDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<EstateModel> CreateEstateAsync(EstateModel estateData, string ownerAuth0Id, CancellationToken cancellationToken = default);

        Task<EstateModel> UpdateEstateAsync(EstateModel estate, string ownerAuth0Id, CancellationToken cancellationToken = default);

        Task DeleteEstateAsync(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default);
    }
}
