using CatalogueService.BLL.Dto;
using CatalogueService.DAL.Models.Entities;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateService
    {
        public Task<IEnumerable<Estate>> GetEstateList(CancellationToken cancellationToken = default);

        public Task<EstateFullDetails> GetEstateDetails(Guid id, CancellationToken cancellationToken = default);

        public Task<Estate> CreateEstate(EstateToCreate estateData, CancellationToken cancellationToken = default);

        public Task<Estate> UpdateEstate(Estate estate, CancellationToken cancellationToken = default);

        public Task<Estate> DeleteEstate(Guid id, CancellationToken cancellationToken = default);
    }
}
