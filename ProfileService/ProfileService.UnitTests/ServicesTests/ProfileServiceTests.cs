using AutoMapper;
using FluentAssertions;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Mapping;
using ProfileService.DAL.Models;
using ProfileService.DAL.Models.Enums;
using ProfileService.UnitTests.Datageneration;
using ProfileService.UnitTests.Mocks;
using ProfileService.UnitTests.Utilities.Mapping;

namespace ProfileService.UnitTests.ServicesTests
{
    public class ProfileServiceTests
    {
        private readonly ProfileRepositoryMock _profileRepositoryMock = new();

        private readonly ProfileInfoVisibilityRepositoryMock _infoVisibilityRepositoryMock = new();

        private readonly CacheRepositoryManagerMock<UserProfile> _profileCacheRepositoryManagerMock = new();

        private readonly CacheRepositoryManagerMock<ProfileInfoVisibility> _visibilityCacheRepositoryManagerMock = new();

        private readonly EstateGrpcClientMock _estateGrpcClientMock = new();

        private readonly List<UserProfile> _profiles = DataGenerator.GenerateUserProfiles(5);

        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([new AutoMapperProfile(), new TestsAutoMapperProfile()])));

        public ProfileServiceTests()
        {
            _profileRepositoryMock.GetByIdAsync(_profiles[0]);
            _profileRepositoryMock.GetByFilterAsync(_profiles[0]);
            _profileRepositoryMock.GetAllAsync(_profiles);
            _profileRepositoryMock.CreateAsync(_profiles[0]);

            _infoVisibilityRepositoryMock.GetAllAsync(_profiles.Select(p => p.InfoVisibility));
            _infoVisibilityRepositoryMock.GetByIdAsync(_profiles[0].InfoVisibility);
            _infoVisibilityRepositoryMock.GetByFilterAsync(_profiles[0].InfoVisibility);
        }

        [Fact]
        public async Task CreateProfileAsync_ProfileModel_ReturnsCreatedProfile()
        {
            // Arrange
            var newProfile = DataGenerator.GenerateUserProfiles(1)[0];
            var correctModel = _mapper.Map<UserProfileModel>(newProfile);

            _profileRepositoryMock.GetByFilterAsyncReturnsNull();
            _profileRepositoryMock.CreateAsync(newProfile);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            //Act
            var response = await service.CreateProfileAsync(_mapper.Map<RegistrationDataModel>(correctModel), default);

            //Assert
            response.Should().BeEquivalentTo(correctModel);
        }

        [Fact]
        public async Task CreateProfileAsync_ExistentProfileModel_ThrowsBadRequestException()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModel>(_profiles[0]);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            //Act
            var response = async () => await service.CreateProfileAsync(_mapper.Map<RegistrationDataModel>(correctModel), default);

            //Assert
            await response.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetProfileAsync_NonExistentProfile_ThrowsNotFoundException()
        {
            // Arrange
            _profileCacheRepositoryManagerMock.GetEntityByIdAsyncReturnsNull();

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.GetProfileAsync(Guid.NewGuid());

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetProfileAsync_NoProfileVisibility_ThrowsNotFoundException()
        {
            // Arrange
            _visibilityCacheRepositoryManagerMock.GetEntityByFilterAsyncReturnsNull();

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.GetProfileAsync(Guid.NewGuid());

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetProfileAsync_ValidId_ReturnsProfileWithPrivacy()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModelWithPrivacy>(_profiles[0]);

            if (_profiles[0].InfoVisibility.BirthDateVisibility == InfoVisibility.Private)
            {
                correctModel.BirthDate = null;
            }

            if (_profiles[0].InfoVisibility.PhoneNumberVisibility == InfoVisibility.Private)
            {
                correctModel.PhoneNumber = null;
            }

            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[0]);
            _visibilityCacheRepositoryManagerMock.GetEntityByFilterAsync(_profiles[0].InfoVisibility);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = await service.GetProfileAsync(_profiles[0].Id);

            // Assert
            response.Should().BeEquivalentTo(correctModel);
        }

        [Fact]
        public async Task GetOwnProfileAsync_ValidId_ReturnsProfile()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModel>(_profiles[0]);

            _profileCacheRepositoryManagerMock.GetEntityByFilterAsync(_profiles[0]);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = await service.GetOwnProfileAsync(_profiles[0].Auth0Id);

            // Assert
            response.Should().BeEquivalentTo(correctModel);
        }

        [Fact]
        public async Task GetOwnProfileAsync_NonExistentProfile_ThrowsNotFoundException()
        {
            // Arrange
            _profileCacheRepositoryManagerMock.GetEntityByFilterAsyncReturnsNull();

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.GetOwnProfileAsync(_profiles[0].Auth0Id);

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateProfileAsync_InvalidId_ThrowsBadRequestException()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModel>(_profiles[0]);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.UpdateProfileAsync(_profiles[1].Id, correctModel, correctModel.Auth0Id, default);

            // Assert
            await response.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateProfileAsync_NonExistentProfile_ThrowsNotFoundException()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModel>(_profiles[0]);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsyncReturnsNull();

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.UpdateProfileAsync(_profiles[0].Id, correctModel, correctModel.Auth0Id, default);

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateProfileAsync_OtherPersonProfile_ThrowsForbiddenException()
        {
            // Arrange
            var correctModel = _mapper.Map<UserProfileModel>(_profiles[0]);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[0]);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = async () => await service.UpdateProfileAsync(_profiles[0].Id, correctModel, _profiles[1].Auth0Id, default);

            // Assert
            await response.Should().ThrowAsync<ForbiddenException>();
        }

        [Fact]
        public async Task UpdateProfileAsync_ValidProfile_ReturnsUpdatedProfile()
        {
            // Arrange
            var updatedProfile = _profiles[1];

            updatedProfile.Id = _profiles[0].Id;
            updatedProfile.Auth0Id = _profiles[0].Auth0Id;

            var updatedProfileModel = _mapper.Map<UserProfileModel>(updatedProfile);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[0]);
            _profileCacheRepositoryManagerMock.UpdateEntityAsync(updatedProfile);

            var service = new UserProfileService(
                _profileRepositoryMock.Object,
                _infoVisibilityRepositoryMock.Object,
                _estateGrpcClientMock.Object,
                _mapper,
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object);

            // Act
            var response = await service.UpdateProfileAsync(updatedProfileModel.Id, updatedProfileModel, updatedProfileModel.Auth0Id, default);

            // Assert
            response.Should().BeEquivalentTo(updatedProfileModel);
        }
    }
}
