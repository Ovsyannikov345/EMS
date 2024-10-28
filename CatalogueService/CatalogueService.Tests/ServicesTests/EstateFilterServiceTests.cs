using AutoFixture.Xunit2;
using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.DAL.Models.Entities;
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
    }
}
