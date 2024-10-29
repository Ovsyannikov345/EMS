using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Community.AutoMapper;
using AutoFixture.Xunit2;
using CatalogueService.BLL.Utilities.Mapping;

namespace CatalogueService.Tests.DataInjection
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoNSubstituteCustomization())
                .Customize(new AutoMapperCustomization(x => x.AddMaps(typeof(AutoMapperProfile)))))
        {
        }
    }
}
