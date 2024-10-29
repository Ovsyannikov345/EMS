using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Community.AutoMapper;
using AutoFixture.Xunit2;
using ChatService.BLL.Utilities.Mapping;
using ChatService.Tests.Customizations;

namespace ChatService.Tests.DataInjection
{
    internal class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
                : base(() => new Fixture()
                    .Customize(new AutoNSubstituteCustomization())
                    .Customize(new AutoMapperCustomization(x => x.AddMaps(typeof(AutoMapperProfile))))
                    .Customize(new ProtoProfileCustomization())
                    .Customize(new ProtoEstateCustomization()))
        {
        }
    }
}
