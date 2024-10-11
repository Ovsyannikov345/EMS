using CatalogueService.BLL.Models;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateService
    {
        public Task<IEnumerable<EstateModel>> GetEstateList(CancellationToken cancellationToken = default);

        public Task<EstateWithProfileModel> GetEstateDetails(Guid id, CancellationToken cancellationToken = default);

        public Task<EstateModel> CreateEstate(EstateModel estateData, string ownerAuth0Id, CancellationToken cancellationToken = default);

        public Task<EstateModel> UpdateEstate(EstateModel estate, string ownerAuth0Id, CancellationToken cancellationToken = default);

        public Task DeleteEstate(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default);
    }
}
