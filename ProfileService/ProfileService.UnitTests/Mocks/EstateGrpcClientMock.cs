using Moq;
using ProfileService.DAL.Grpc.Services.IServices;

namespace ProfileService.UnitTests.Mocks
{
    public class EstateGrpcClientMock : Mock<IEstateGrpcClient>
    {
        public void GetEstateCount(int countToReturn) =>
            Setup(c => c.GetEstateCount(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(countToReturn);
    }
}
