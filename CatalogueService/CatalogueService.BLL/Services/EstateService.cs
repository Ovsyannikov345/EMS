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
using CatalogueService.BLL.Utilities.QueryParameters;
using CatalogueService.DAL.Utilities.Pagination;
using System.Linq.Expressions;
using CatalogueService.DAL.Models.Enums;

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

        public async Task<PagedResult<EstateModel>> GetEstateListAsync(
            SortOption sortOption,
            EstateQueryFilter filter,
            Pagination pagination,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Estate, object>> sortParameter = sortOption switch
            {
                SortOption.DateDescending or SortOption.DateAscending => (e) => e.CreatedAt,
                SortOption.PriceDescending or SortOption.PriceAscending => (e) => e.Price,
                SortOption.AreaDescending or SortOption.AreaAscending => (e) => e.Area,
                _ => (e) => e.CreatedAt,
            };

            var isDescending =
                sortOption == SortOption.DateDescending ||
                sortOption == SortOption.PriceDescending ||
                sortOption == SortOption.AreaDescending;

            Expression<Func<Estate, bool>> predicate = (e) =>
                (!filter.Types.HasValue || (e.Type != EstateType.None && (filter.Types & e.Type) == e.Type)) &&
                (string.IsNullOrEmpty(filter.Address) || e.Address.Contains(filter.Address)) &&
                (!filter.MaxPrice.HasValue || e.Price <= filter.MaxPrice) &&
                (!filter.MinPrice.HasValue || e.Price >= filter.MinPrice) &&
                (!filter.MaxArea.HasValue || e.Area <= filter.MaxArea) &&
                (!filter.MinArea.HasValue || e.Area >= filter.MinArea) &&
                (!filter.MaxRoomsCount.HasValue || e.RoomsCount <= filter.MaxRoomsCount) &&
                (!filter.MinRoomsCount.HasValue || e.RoomsCount >= filter.MinRoomsCount);

            var result = await estateRepository.GetAllAsync(
                sortParameter, isDescending, predicate, pagination.PageNumber, pagination.PageSize, cancellationToken);

            var resultWithModels = mapper.Map<PagedResult<EstateModel>>(result);

            resultWithModels.Results = await AddImageIdsToEstate(resultWithModels.Results, cancellationToken);

            return resultWithModels;
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

        private async Task<List<EstateModel>> AddImageIdsToEstate(List<EstateModel> estateModels, CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < estateModels.Count; i++)
            {
                estateModels[i].ImageIds = await estateImageService.GetImageNameListAsync(estateModels[i].Id, cancellationToken);
            }

            return estateModels;
        }
    }
}
