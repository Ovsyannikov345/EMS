using AutoFixture.Xunit2;
using AutoMapper;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.Tests.DataInjection;
using CatalogueService.Tests.Mapping;
using FluentAssertions;
using Grpc.Core;
using NSubstitute;

namespace CatalogueService.Tests.ServicesTests
{
    public class EstateGrpcServiceTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([new AutoMapperProfile(), new TestMapperProfile()])));

        [Theory]
        [AutoDomainData]
        public async Task GetEstate_InvalidId_ThrowsBadRequestException(
            EstateRequest estateRequest,
            ServerCallContext serverCallContext,
            EstateGrpcService sut
            )
        {
            // Arrange
            estateRequest.Id = "asd";

            // Act
            var result = async () => await sut.GetEstate(estateRequest, serverCallContext);

            // Assert
            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstate_ValidId_ThrowsBadRequestException(
            [Frozen] IEstateService estateServiceMock,
            EstateWithProfileModel estateWithProfileModel,
            EstateRequest estateRequest,
            ServerCallContext serverCallContext,
            EstateGrpcService sut
            )
        {
            // Arrange
            estateRequest.Id = estateWithProfileModel.Id.ToString();
            estateServiceMock.GetEstateDetailsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateWithProfileModel);

            var expected = new EstateResponse
            {
                Estate = _mapper.Map<ProtoEstateModel>(estateWithProfileModel),
            };

            // Act
            var result = await sut.GetEstate(estateRequest, serverCallContext);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
