using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utitlities.Exceptions;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories;

namespace CatalogueService.BLL.Services
{
    public class EstateService(EstateRepository estateRepository) : IEstateService
    {
        public Task<Estate> CreateEstate(EstateToCreate estateData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Estate> DeleteEstate(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedEstate = await estateRepository.DeleteAsync(id, cancellationToken);

            if (deletedEstate is null)
            {
                throw new NotFoundException("Estate doesn't exist");
            }

            return deletedEstate;
        }

        public Task<Estate> GetEstateDetails(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Estate>> GetEstateList(CancellationToken cancellationToken = default)
        {
            return await estateRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Estate> UpdateEstate(Estate estate, CancellationToken cancellationToken = default)
        {
            if (!await estateRepository.Exists(e => e.Id == estate.Id, cancellationToken))
            {
                throw new NotFoundException("Estate not found");
            }

            await estateRepository.UpdateAsync(estate, cancellationToken);

            return estate;
        }
    }
}
