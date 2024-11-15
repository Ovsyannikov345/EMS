using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Messages;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;
using CatalogueService.BLL.Producers.IProducers;
using MessageBus.Messages;
using CatalogueService.BLL.Utilities.Exceptions.Messages;

namespace CatalogueService.BLL.Services
{
    public class EstateService(
        IEstateRepository estateRepository,
        IProfileGrpcClient profileGrpcClient,
        IEstateFilterService estateFilterService,
        IEstateImageService estateImageService,
        INotificationProducer notificationProducer,
        IMapper mapper) : IEstateService
    {
        public async Task<EstateModel> CreateEstateAsync(EstateModel estateData, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            var ownerProfile = await profileGrpcClient.GetOwnProfile(ownerAuth0Id, cancellationToken);

            estateData.UserId = ownerProfile.Id;

            var createdEstate = await estateRepository.CreateAsync(mapper.Map<Estate>(estateData), cancellationToken);

            var estateModel = mapper.Map<EstateModel>(createdEstate);

            var userFilters = await estateFilterService.GetEstateFiltersAsync(estateModel, cancellationToken);

            foreach (var filter in userFilters)
            {
                await notificationProducer.SendNotification(new CreateNotification
                {
                    Title = NotificationMessages.NewEstate,
                    UserId = filter.UserId,
                }, cancellationToken);
            }

            return estateModel;
        }

        public async Task DeleteEstateAsync(Guid id, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwnerAsync(id, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(ExceptionMessages.DeleteDenied(nameof(Estate), id));
            }

            if (!await estateRepository.DeleteAsync(id, cancellationToken))
            {
                throw new NotFoundException(ExceptionMessages.NotFound(nameof(Estate), nameof(Estate.Id), id));
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

                estateFullDetails.ImageIds = await estateImageService.GetImageNameListAsync(estate.Id, cancellationToken);

                return estateFullDetails;
            }

            throw new NotFoundException(ExceptionMessages.NotFound(nameof(Estate), nameof(Estate.Id), id));
        }

        public async Task<IEnumerable<EstateModel>> GetEstateListAsync(CancellationToken cancellationToken = default)
        {
            var estates = await estateRepository.GetAllAsync(cancellationToken: cancellationToken);

            var estateModels = mapper.Map<IEnumerable<Estate>, IEnumerable<EstateModel>>(estates).ToList();

            for (var i = 0; i < estateModels.Count; i++)
            {
                estateModels[i].ImageIds = await estateImageService.GetImageNameListAsync(estateModels[i].Id, cancellationToken);
            }

            return estateModels;
        }

        public async Task<EstateModel> UpdateEstateAsync(EstateModel estate, string ownerAuth0Id, CancellationToken cancellationToken = default)
        {
            if (!await IsUserEstateOwnerAsync(estate.Id, ownerAuth0Id, cancellationToken))
            {
                throw new ForbiddenException(ExceptionMessages.UpdateDenied(nameof(Estate), estate.Id));
            }

            var updatedEstate = await estateRepository.UpdateAsync(mapper.Map<Estate>(estate), cancellationToken);

            return mapper.Map<EstateModel>(updatedEstate);
        }

        private async Task<bool> IsUserEstateOwnerAsync(Guid estateId, string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByFilterAsync(e => e.Id == estateId, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(Estate), nameof(Estate.Id), estateId));

            var owner = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            return estate.UserId == owner.Id;
        }
    }
}
