using AutoMapper;
using ProfileService.BLL.Dto;
using ProfileService.DAL.Models;

namespace ProfileService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationData, UserProfile>()
                .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
