using AutoFixture.Xunit2;
using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Models.Enums;
using CatalogueService.DAL.Repositories.IRepositories;
using CatalogueService.Tests.DataInjection;
using CatalogueService.Tests.Mapping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace CatalogueService.Tests.ServicesTests
{
    public class EstateFilterServiceTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([new AutoMapperProfile(), new TestMapperProfile()])));

        [Theory]
        [AutoDomainData]
        public async Task GetEstateFilterAsync_NonExistentFilter_ThrowsNotFoundException(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);
            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var result = async () => await sut.GetEstateFilterAsync(userProfile.Auth0Id, default);

            // TODO add asserts
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateFilterAsync_ValidFilter_ReturnsFilter(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            UserProfile userProfile,
            EstateFilter estateFilter,
            EstateFilterService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>())
                .Returns(userProfile);
            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);

            // Act
            var result = await sut.GetEstateFilterAsync(userProfile.Auth0Id, default);

            result.Should().BeEquivalentTo(_mapper.Map<EstateFilterModel>(estateFilter));
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateFiltersAsync__ReturnsFilterList(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            Estate estate,
            List<EstateFilter> estateFilters,
            EstateFilterService sut)
        {
            // Arrange
            estateFilterRepositoryMock.GetAllAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilters);

            // Act
            var result = await sut.GetEstateFiltersAsync(_mapper.Map<EstateModel>(estate), default);

            result.Should().BeEquivalentTo(_mapper.Map<IEnumerable<EstateFilter>, IEnumerable<EstateFilterModel>>(estateFilters));
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateEstateFilterAsync_FilterExists_ThrowsBadRequestException(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilter.UserId = userProfile.Id;

            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.CreateEstateFilterAsync(userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateEstateFilterAsync_FilterNotExists_ReturnsCreatedFilter(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilter.UserId = userProfile.Id;

            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();
            estateFilterRepositoryMock.CreateAsync(Arg.Any<EstateFilter>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = await sut.CreateEstateFilterAsync(userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            result.Should().BeEquivalentTo(_mapper.Map<EstateFilterModel>(estateFilter));
        }

        [Theory]
        [AutoDomainData]
        public async Task UpdateEstateFilterAsync_InvalidId_ThrowsBadRequestException(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.UpdateEstateFilterAsync(Guid.NewGuid(), userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task UpdateEstateFilterAsync_FilterNotExists_ThrowsNotFoundException(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.UpdateEstateFilterAsync(estateFilter.Id, userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task UpdateEstateFilterAsync_OtherPersonFilter_ThrowsForbiddenException(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.UpdateEstateFilterAsync(estateFilter.Id, userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            await result.Should().ThrowAsync<ForbiddenException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task UpdateEstateFilterAsync_ValidFilter_ReturnsUpdatedFilter(
            [Frozen] IEstateFilterRepository estateFilterRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateFilter estateFilter,
            UserProfile userProfile,
            EstateFilterService sut)
        {
            // Arrange
            estateFilter.UserId = userProfile.Id;

            estateFilterRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<EstateFilter, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            estateFilterRepositoryMock.UpdateAsync(Arg.Any<EstateFilter>(), Arg.Any<CancellationToken>())
                .Returns(estateFilter);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = await sut.UpdateEstateFilterAsync(estateFilter.Id, userProfile.Auth0Id, _mapper.Map<EstateFilterModel>(estateFilter), default);

            result.Should().BeEquivalentTo(estateFilter);
        }
    }
}
