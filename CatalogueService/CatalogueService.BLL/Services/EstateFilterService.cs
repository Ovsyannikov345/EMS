using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Exceptions.Messages;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

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
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(EstateFilter), nameof(EstateFilter.UserId), profile.Id));

            return mapper.Map<EstateFilterModel>(filter);
        }

        public async Task<IEnumerable<EstateFilterModel>> GetEstateFiltersAsync(EstateModel estate, CancellationToken cancellationToken = default)
        {
            Expression<Func<EstateFilter, bool>> query = (EstateFilter e) =>
                estate.Price >= e.MinPrice && estate.Price <= e.MaxPrice &&
                estate.RoomsCount >= e.MinRoomsCount && estate.RoomsCount <= e.MaxRoomsCount &&
                estate.Area >= e.MinArea && estate.Area <= e.MaxArea &&
                e.EstateTypes.HasFlag(estate.Type);

            var filters = await estateFilterRepository.GetAllAsync(query, cancellationToken);

            return mapper.Map<IEnumerable<EstateFilter>, IEnumerable<EstateFilterModel>>(filters);
        }

        public async Task<EstateFilterModel> CreateEstateFilterAsync(string userAuth0Id, EstateFilterModel filterData, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            var filter = await estateFilterRepository.GetByFilterAsync(e => e.UserId == profile.Id, cancellationToken);

            if (filter is not null)
            {
                throw new BadRequestException(ExceptionMessages.AlreadyExists(nameof(EstateFilter), nameof(EstateFilter.UserId), profile.Id));
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
                throw new BadRequestException(ExceptionMessages.InvalidId(nameof(EstateFilter), id));
            }

            var filter = await estateFilterRepository.GetByFilterAsync(e => e.Id == filterData.Id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(EstateFilter), nameof(EstateFilter.UserId), filterData.Id));

            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (filter.UserId != profile.Id)
            {
                throw new ForbiddenException(ExceptionMessages.UpdateDenied(nameof(EstateFilter), filter.Id));
            }

            var updatedFilter = await estateFilterRepository.UpdateAsync(mapper.Map<EstateFilter>(filterData), cancellationToken);

            return mapper.Map<EstateFilterModel>(updatedFilter);
        }
    }
}
