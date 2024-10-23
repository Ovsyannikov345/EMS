using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Messages;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;

namespace CatalogueService.BLL.Services
{
    public class EstateFilterService(
        IEstateFilterRepository estateFilterRepository,
        IProfileGrpcClient profileGrpcClient,
        IMapper mapper) : IEstateFilterService
    {
        public async Task<EstateFilterModel> GetEstateFilterAsync(string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            var filter = await estateFilterRepository.GetByFilterAsync(e => e.UserId == profile.Id, cancellationToken)
                ?? throw new NotFoundException(EstateFilterMessages.NotFound);

            return mapper.Map<EstateFilterModel>(filter);
        }

        public async Task<IEnumerable<EstateFilterModel>> GetEstateFiltersAsync(EstateModel estate, CancellationToken cancellationToken = default)
        {
            var filters = await estateFilterRepository.GetAllAsync(e =>
                estate.Price >= e.MinPrice && estate.Price <= e.MaxPrice &&
                estate.RoomsCount >= e.MinRoomsCount && estate.RoomsCount <= e.MaxRoomsCount &&
                estate.Area >= e.MinArea && estate.Area <= e.MaxArea &&
                e.EstateTypes.HasFlag(estate.Type), cancellationToken);

            return mapper.Map<IEnumerable<EstateFilter>, IEnumerable<EstateFilterModel>>(filters);
        }

        public async Task<EstateFilterModel> CreateEstateFilterAsync(string userAuth0Id, EstateFilterModel filterData, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            var filter = await estateFilterRepository.GetByFilterAsync(e => e.UserId == profile.Id, cancellationToken);

            if (filter is not null)
            {
                throw new BadRequestException(EstateFilterMessages.AlreadyExists);
            }

            var filterToCreate = mapper.Map<EstateFilter>(filterData);

            filterToCreate.UserId = profile.Id;

            var createdFilter = await estateFilterRepository.CreateAsync(filterToCreate, cancellationToken);

            return mapper.Map<EstateFilterModel>(createdFilter);
        }

        public async Task<EstateFilterModel> UpdateEstateFilterAsync(Guid id, string userAuth0Id, EstateFilterModel filterData, CancellationToken cancellationToken = default)
        {
            if (id != filterData.Id)
            {
                throw new BadRequestException(EstateFilterMessages.InvalidId);
            }

            var filter = await estateFilterRepository.GetByFilterAsync(e => e.Id == filterData.Id, cancellationToken)
                ?? throw new NotFoundException(EstateFilterMessages.NotFound);

            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (filter.UserId != profile.Id)
            {
                throw new ForbiddenException(EstateFilterMessages.AccessDenied);
            }

            var updatedFilter = await estateFilterRepository.UpdateAsync(mapper.Map<EstateFilter>(filterData), cancellationToken);

            return mapper.Map<EstateFilterModel>(updatedFilter);
        }
    }
}
