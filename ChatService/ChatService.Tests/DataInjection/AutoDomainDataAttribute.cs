using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Community.AutoMapper;
using AutoFixture.Xunit2;
using ChatService.Tests.Customizations;
using ApiAutoMapperProfile = ChatService.Utilities.Mapping.AutoMapperProfile;
using BLLAutoMapperProfile = ChatService.BLL.Utilities.Mapping.AutoMapperProfile;

namespace ChatService.Tests.DataInjection
{
    internal class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
                : base(() => new Fixture()
                    .Customize(new AutoNSubstituteCustomization())
                    .Customize(new AutoMapperCustomization(x =>
                        x.AddMaps(typeof(ApiAutoMapperProfile), typeof(BLLAutoMapperProfile))))
                    .Customize(new ProtoProfileCustomization())
                    .Customize(new ProtoEstateCustomization()))
        {
        }
    }
}
