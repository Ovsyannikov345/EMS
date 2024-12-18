﻿using AutoMapper;
using FluentAssertions;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Mapping;
using ProfileService.DAL.Models;
using ProfileService.UnitTests.Datageneration;
using ProfileService.UnitTests.Mocks;

namespace ProfileService.UnitTests.ServicesTests
{
    public class ProfileVisibilityTests
    {
        private readonly CacheRepositoryManagerMock<UserProfile> _profileCacheRepositoryManagerMock = new();

        private readonly CacheRepositoryManagerMock<ProfileInfoVisibility> _visibilityCacheRepositoryManagerMock = new();

        private readonly List<UserProfile> _profiles = DataGenerator.GenerateUserProfiles(5);

        private readonly List<ProfileInfoVisibility> _visibilities = DataGenerator.GenerateProfileInfoVisibilities(5);

        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfile(new AutoMapperProfile())));

        public ProfileVisibilityTests()
        {
            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[0]);
            _profileCacheRepositoryManagerMock.GetEntityByFilterAsync(_profiles[0]);
            _visibilityCacheRepositoryManagerMock.GetEntityByFilterAsync(_visibilities[0]);

            for (var i = 0; i < _profiles.Count; i++)
            {
                _profiles[i].InfoVisibility = _visibilities[i];
                _visibilities[i].UserId = _profiles[i].Id;
                _visibilities[i].User = _profiles[i];
            }
        }

        [Fact]
        public async Task GetProfileInfoVisibilityAsync_NonExistentVisibility_ThrowsNotFoundException()
        {
            // Arrange
            _visibilityCacheRepositoryManagerMock.GetEntityByFilterAsyncReturnsNull();

            var profileVisibilityService = new ProfileInfoVisibilityService(
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object,
                _mapper);

            // Act
            var response = async () => await profileVisibilityService.GetProfileInfoVisibilityAsync(_visibilities[0].UserId, default);

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetProfileInfoVisibilityAsync_ValidId_ReturnsVisibility()
        {
            // Arrange
            var correctModel = _mapper.Map<ProfileInfoVisibilityModel>(_visibilities[0]);

            var profileVisibilityService = new ProfileInfoVisibilityService(
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object,
                _mapper);

            // Act
            var response = await profileVisibilityService.GetProfileInfoVisibilityAsync(_visibilities[0].UserId, default);

            // Assert
            response.Should().BeEquivalentTo(correctModel);
        }

        [Fact]
        public async Task UpdateProfileInfoVisibilityAsync_InvalidId_ThrowsBadRequestException()
        {
            // Arrange
            var visibilityModel = _mapper.Map<ProfileInfoVisibilityModel>(_visibilities[0]);

            var profileVisibilityService = new ProfileInfoVisibilityService(
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object,
                _mapper);

            // Act
            var response = async () => await profileVisibilityService.UpdateProfileInfoVisibilityAsync(_profiles[0].Auth0Id, _profiles[1].Id, visibilityModel, default);

            // Assert
            await response.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateProfileInfoVisibilityAsync_NonExistentProfile_ThrowsNotFoundException()
        {
            // Arrange
            var visibilityModel = _mapper.Map<ProfileInfoVisibilityModel>(_visibilities[0]);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsyncReturnsNull();

            var profileVisibilityService = new ProfileInfoVisibilityService(
                 _profileCacheRepositoryManagerMock.Object,
                 _visibilityCacheRepositoryManagerMock.Object,
                 _mapper);

            // Act
            var response = async () => await profileVisibilityService.UpdateProfileInfoVisibilityAsync(_profiles[0].Auth0Id, visibilityModel.UserId, visibilityModel, default);

            // Assert
            await response.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateProfileInfoVisibilityAsync_OtherPersonVisibility_ThrowsForbiddenException()
        {
            // Arrange
            var visibilityModel = _mapper.Map<ProfileInfoVisibilityModel>(_visibilities[0]);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[1]);

            var profileVisibilityService = new ProfileInfoVisibilityService(
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object,
                _mapper);

            // Act
            var response = async () => await profileVisibilityService.UpdateProfileInfoVisibilityAsync(_profiles[1].Auth0Id, visibilityModel.UserId, visibilityModel, default);

            // Assert
            await response.Should().ThrowAsync<ForbiddenException>();
        }

        [Fact]
        public async Task UpdateProfileInfoVisibilityAsync_ValidVisibility_ReturnsUpdatedvisibility()
        {
            // Arrange
            var updatedVisibility = DataGenerator.GenerateProfileInfoVisibilities(1)[0];

            updatedVisibility.Id = _visibilities[0].Id;
            updatedVisibility.UserId = _visibilities[0].UserId;

            var updatedVisibilityModel = _mapper.Map<ProfileInfoVisibilityModel>(updatedVisibility);

            _profileCacheRepositoryManagerMock.GetEntityByIdAsync(_profiles[0]);
            _visibilityCacheRepositoryManagerMock.UpdateEntityAsync(updatedVisibility);

            var profileVisibilityService = new ProfileInfoVisibilityService(
                _profileCacheRepositoryManagerMock.Object,
                _visibilityCacheRepositoryManagerMock.Object,
                _mapper);

            // Act
            var response = await profileVisibilityService.UpdateProfileInfoVisibilityAsync(_profiles[0].Auth0Id, updatedVisibilityModel.UserId, updatedVisibilityModel, default);

            // Assert
            response.Should().BeEquivalentTo(updatedVisibility);
        }
    }
}
