using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Community.AutoMapper;
using AutoFixture.Xunit2;
using NotificationService.BLL.Utilities.Mapping;

namespace NotificationService.Tests.DataInjection
{
    internal class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoNSubstituteCustomization())
                .Customize(new AutoMapperCustomization(x => x.AddMaps(typeof(AutoMapperProfile)))))
        {
        }
    }
}
