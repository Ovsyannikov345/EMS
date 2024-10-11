using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utitlities.Messages;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;

namespace CatalogueService.BLL.Services
{
    public class EstateService(IEstateRepository estateRepository, IProfileGrpcClient profileGrpcClient, IMapper mapper) : IEstateService
    {
        public async Task<EstateModel> CreateEstateAsync(EstateModel estateData, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            var ownerProfile = await profileGrpcClient.GetOwnProfile(ownerAuth0Id, cancellationToken);

            estateData.UserId = ownerProfile.Id;

            var createdEstate = await estateRepository.CreateAsync(mapper.Map<Estate>(estateData), cancellationToken);

            return mapper.Map<EstateModel>(createdEstate);
        }

        public async Task DeleteEstateAsync(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwnerAsync(id, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(EstateMessages.EstateDeleteForbidden);
            }

            if (!await estateRepository.DeleteAsync(id, cancellationToken))
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }
        }

        public async Task<EstateWithProfileModel> GetEstateDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByFilterAsync(e => e.Id == id, cancellationToken);

            if (estate is not null)
            {
                var estateFullDetails = mapper.Map<EstateWithProfileModel>(estate);

                var estateOwnerProfile = await profileGrpcClient.GetProfile(estateFullDetails.UserId, cancellationToken);

                estateFullDetails.User = mapper.Map<UserProfileModel>(estateOwnerProfile);

                return estateFullDetails;
            }

            throw new NotFoundException(EstateMessages.EstateNotFound);

        }

        public async Task<IEnumerable<EstateModel>> GetEstateListAsync(CancellationToken cancellationToken = default)
        {
            var estates = await estateRepository.GetAllAsync(cancellationToken);

            return mapper.Map<IEnumerable<Estate>, IEnumerable<EstateModel>>(estates);
        }

        public async Task<EstateModel> UpdateEstateAsync(EstateModel estate, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwnerAsync(estate.Id, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(EstateMessages.EstateUpdateForbidden);
            }

            var updatedEstate = await estateRepository.UpdateAsync(mapper.Map<Estate>(estate), cancellationToken);

            return mapper.Map<EstateModel>(updatedEstate);
        }

        private async Task<bool> IsUserEstateOwnerAsync(Guid estateId, string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByFilterAsync(e => e.Id == estateId, cancellationToken);

            if (estate is null)
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            var owner = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            return estate.UserId == owner.Id;
        }
    }
}
