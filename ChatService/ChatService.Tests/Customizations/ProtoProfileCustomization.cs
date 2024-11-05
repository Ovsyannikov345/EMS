using AutoFixture;

namespace ChatService.Tests.Customizations
{
    internal class ProtoProfileCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<DAL.Grpc.Services.Profile.ProtoProfileModel>(composer =>
                composer.With(p => p.Id, Guid.NewGuid().ToString()));
            fixture.Customize<DAL.Grpc.Services.Estate.ProtoProfileModel>(composer =>
                composer.With(p => p.Id, Guid.NewGuid().ToString()));
        }
    }
}
