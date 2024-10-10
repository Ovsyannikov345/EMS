using AutoMapper;
using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Grpc.Services.IServices;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utitlities.Messages;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;

namespace CatalogueService.BLL.Services
{
    public class EstateService(IEstateRepository estateRepository, IProfileGrpcClient profileGrpcClient, IMapper mapper) : IEstateService
    {
        public Task<Estate> CreateEstate(EstateToCreate estateData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Estate> DeleteEstate(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwner(id, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(EstateMessages.EstateDeleteForbidden);
            }

            var deletedEstate = await estateRepository.DeleteAsync(id, cancellationToken);

            return deletedEstate!;
        }

        public async Task<EstateFullDetails> GetEstateDetails(Guid id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByIdAsync(id, cancellationToken);

            if (estate is null)
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            var estateFullDetails = mapper.Map<EstateFullDetails>(estate);

            estateFullDetails.User = await profileGrpcClient.GetProfile(estateFullDetails.UserId, cancellationToken);

            return estateFullDetails;
        }

        public async Task<IEnumerable<Estate>> GetEstateList(CancellationToken cancellationToken = default)
        {
            return await estateRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Estate> UpdateEstate(Estate estate, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwner(estate.UserId, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(EstateMessages.EstateUpdateForbidden);
            }

            await estateRepository.UpdateAsync(estate, cancellationToken);

            return estate;
        }

        private async Task<bool> IsUserEstateOwner(Guid estateId, string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByIdAsync(estateId, cancellationToken);

            if (estate is null)
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            var owner = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            return estate.UserId == owner.Id;
        }
    }
}
