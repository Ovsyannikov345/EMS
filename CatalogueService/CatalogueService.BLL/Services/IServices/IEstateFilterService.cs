using CatalogueService.BLL.Models;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateFilterService
    {
        Task<EstateFilterModel> GetEstateFilterAsync(string userAuth0Id, CancellationToken cancellationToken = default);

        Task<IEnumerable<EstateFilterModel>> GetEstateFiltersAsync(EstateModel estate, CancellationToken cancellationToken = default);

        Task<EstateFilterModel> CreateEstateFilterAsync(string userAuth0Id, EstateFilterModel filterData, CancellationToken cancellationToken = default);

        Task<EstateFilterModel> UpdateEstateFilterAsync(Guid id, string userAuth0Id, EstateFilterModel filterData, CancellationToken cancellationToken = default);
    }
}