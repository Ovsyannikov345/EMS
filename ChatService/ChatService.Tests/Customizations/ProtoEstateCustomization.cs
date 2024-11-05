using AutoFixture;
using ChatService.DAL.Grpc.Services.Estate;

namespace ChatService.Tests.Customizations
{
    internal class ProtoEstateCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ProtoEstateModel>(composer =>
                composer.With(e => e.Id, Guid.NewGuid().ToString())
                        .With(e => e.Price, "10000"));
        }
    }
}
