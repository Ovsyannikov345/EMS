using AutoMapper;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Testing;
using ProfileService.BLL.Grpc.Services;
using ProfileService.BLL.Models;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Mapping;
using ProfileService.DAL.Models;
using ProfileService.UnitTests.Datageneration;
using ProfileService.UnitTests.Mocks;

namespace ProfileService.UnitTests.ServicesTests
{
    public class ProfileGrpcServiceTests
    {
        private readonly ProfileServiceMock _profileServiceMock = new();

        private readonly List<UserProfile> _profiles = DataGenerator.GenerateUserProfiles(5);

        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfile(new AutoMapperProfile())));

        [Fact]
        public async Task GetProfile_InvalidId_ThrowsbadRequestException()
        {
            // Arrange
            var request = new ProfileRequest { Id = "asd" };

            var profileGrpcService = new ProfileGrpcService(_profileServiceMock.Object, _mapper);

            // Act
            var response = async () => await profileGrpcService.GetProfile(request,
                DataGenerator.GenerateServerCallContext(nameof(ProfileGrpcService.GetProfile)));

            // Assert
            await response.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetProfile_ValidId_ReturnsProfile()
        {
            // Arrange
            var request = new ProfileRequest { Id = _profiles[0].Id.ToString() };

            _profileServiceMock.GetProfileAsync(_mapper.Map<UserProfileModelWithPrivacy>(_profiles[0]));

            var profileGrpcService = new ProfileGrpcService(_profileServiceMock.Object, _mapper);

            var expected = new ProfileResponse
            {
                Profile = _mapper.Map<ProtoProfileModel>(_profiles[0])
            };

            // Act
            var response = await profileGrpcService.GetProfile(request,
                DataGenerator.GenerateServerCallContext(nameof(ProfileGrpcService.GetProfile)));

            // Assert
            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetOwnProfile_ValidId_ReturnsProfile()
        {
            // Arrange
            var request = new OwnProfileRequest { Auth0Id = _profiles[0].Auth0Id };

            _profileServiceMock.GetOwnProfileAsync(_mapper.Map<UserProfileModel>(_profiles[0]));

            var profileGrpcService = new ProfileGrpcService(_profileServiceMock.Object, _mapper);

            var expected = new ProfileResponse
            {
                Profile = _mapper.Map<ProtoProfileModel>(_profiles[0])
            };

            // Act
            var response = await profileGrpcService.GetOwnProfile(request,
                DataGenerator.GenerateServerCallContext(nameof(ProfileGrpcService.GetOwnProfile)));

            // Assert
            response.Should().BeEquivalentTo(expected);
        }
    }
}
