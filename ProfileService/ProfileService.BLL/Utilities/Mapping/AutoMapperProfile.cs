using AutoMapper;
using ProfileService.BLL.Dto;
using ProfileService.DAL.Models;
using ProfileService.BLL.Grpc.Services;
using Google.Protobuf.WellKnownTypes;

namespace ProfileService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationData, UserProfile>()
                .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserProfile, ProfileResponse>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc))));
        }
    }
}
