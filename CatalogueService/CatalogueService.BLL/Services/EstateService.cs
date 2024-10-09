using AutoMapper;
using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utitlities.Messages;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories;

namespace CatalogueService.BLL.Services
{
    public class EstateService(EstateRepository estateRepository, ProfileGrpcClient profileGrpcClient, IMapper mapper) : IEstateService
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
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            return deletedEstate;
        }

        public async Task<EstateFullDetails> GetEstateDetails(Guid id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByIdAsync(id, cancellationToken);

            var estateFullDetails = mapper.Map<EstateFullDetails>(estate);

            estateFullDetails.User = await profileGrpcClient.GetProfile(estateFullDetails.UserId);

            return estateFullDetails;
        }

        public async Task<IEnumerable<Estate>> GetEstateList(CancellationToken cancellationToken = default)
        {
            return await estateRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Estate> UpdateEstate(Estate estate, CancellationToken cancellationToken = default)
        {
            if (!await estateRepository.Exists(e => e.Id == estate.Id, cancellationToken))
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            await estateRepository.UpdateAsync(estate, cancellationToken);

            return estate;
        }
    }
}
