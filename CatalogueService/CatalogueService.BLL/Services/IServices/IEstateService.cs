using CatalogueService.BLL.Models;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateService
    {
        public Task<IEnumerable<EstateModel>> GetEstateListAsync(CancellationToken cancellationToken = default);

        public Task<EstateWithProfileModel> GetEstateDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<EstateModel> CreateEstateAsync(EstateModel estateData, string ownerAuth0Id, CancellationToken cancellationToken = default);

        public Task<EstateModel> UpdateEstateAsync(EstateModel estate, string ownerAuth0Id, CancellationToken cancellationToken = default);

        public Task DeleteEstate(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default);
    }
}
