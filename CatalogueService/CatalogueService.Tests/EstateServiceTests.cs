using AutoFixture.Xunit2;
using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Services.IServices;
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

namespace CatalogueService.Tests
{
    public class EstateServiceTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([new AutoMapperProfile(), new TestMapperProfile()])));

        [Theory]
        [AutoDomainData]
        public async Task CreateEstateAsync_ValidEstate_ReturnsCreatedEstate(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateRepository estateRepositoryMock,
            [Frozen] IEstateFilterService estateFilterServiceMock,
            EstateModel estateModel,
            UserProfile userProfile,
            EstateService sut)
        {
            // Arrange
            estateModel.UserId = userProfile.Id;

            var estateEntity = _mapper.Map<Estate>(estateModel);

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);
            estateRepositoryMock.CreateAsync(Arg.Any<Estate>())
                .Returns(estateEntity);
            estateFilterServiceMock.GetEstateFiltersAsync(Arg.Any<EstateModel>())
                .Returns([]);

            // Act
            var result = await sut.CreateEstateAsync(estateModel, "", default);

            // Assert
            result.Should().BeEquivalentTo(estateModel);
        }

        [Theory]
        [AutoDomainData]
        public async Task DeleteEstateAsync_OtherPersonsEstate_ThrowsForbiddenException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateRepository estateRepositoryMock,
            Estate estate,
            UserProfile userProfile,
            EstateService sut)
        {
            // Arrange
            estateRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .Returns(estate);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.DeleteEstateAsync(estate.Id, "", default);

            // Assert
            await result.Should().ThrowAsync<ForbiddenException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task DeleteEstateAsync_NonExistentEstate_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateRepository estateRepositoryMock,
            Estate estate,
            UserProfile userProfile,
            EstateService sut)
        {
            // Arrange
            estate.UserId = userProfile.Id;

            estateRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .Returns(estate);
            estateRepositoryMock.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(false);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.DeleteEstateAsync(estate.Id, userProfile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task DeleteEstateAsync_ValidEstate_DeletesEstate(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateRepository estateRepositoryMock,
            Estate estate,
            UserProfile userProfile,
            EstateService sut)
        {
            // Arrange
            estate.UserId = userProfile.Id;

            estateRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .Returns(estate);
            estateRepositoryMock.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(true);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(userProfile);

            // Act
            var result = async () => await sut.DeleteEstateAsync(estate.Id, userProfile.Auth0Id, default);

            // Assert
            await result.Should().NotThrowAsync();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateDetailsAsync_NonExistentEstate_ThrowsNotFoundException(
            [Frozen] IEstateRepository estateRepositoryMock,
            Estate estate,
            UserProfile userProfile,
            EstateService sut)
        {
            // Arrange
            estate.UserId = userProfile.Id;

            estateRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .ReturnsNull();

            // Act
            var result = async () => await sut.GetEstateDetailsAsync(estate.Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateDetailsAsync_ValidEstateId_ReturnsEstateDetails(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateRepository estateRepositoryMock,
            EstateWithProfileModel estate,
            EstateService sut)
        {
            // Arrange
            estateRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .Returns(_mapper.Map<Estate>(estate));
            profileGrpcClientMock.GetProfile(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<UserProfile>(estate.User));

            // Act
            var result = await sut.GetEstateDetailsAsync(estate.Id, default);

            // Assert
            result.Should().BeEquivalentTo(estate);
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateListAsync__ReturnsEstateList(
            [Frozen] IEstateRepository estateRepositoryMock,
            IEnumerable<Estate> estateList,
            EstateService sut)
        {
            // Arrange
            estateRepositoryMock.GetAllAsync(Arg.Any<Expression<Func<Estate, bool>>>())
                .Returns(estateList);

            // Act
            var result = await sut.GetEstateListAsync(default);

            // Assert
            result.Should().BeEquivalentTo(_mapper.Map<IEnumerable<Estate>, IEnumerable<EstateModel>>(estateList));
        }
    }
}