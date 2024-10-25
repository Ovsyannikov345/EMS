using AutoMapper;
using ProfileService.BLL.Models;

namespace ProfileService.UnitTests.Utilities.Mapping
{
    internal class TestsAutoMapperProfile : Profile
    {
        public TestsAutoMapperProfile()
        {
            CreateMap<UserProfileModel, RegistrationDataModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Auth0Id));
        }
    }
}
