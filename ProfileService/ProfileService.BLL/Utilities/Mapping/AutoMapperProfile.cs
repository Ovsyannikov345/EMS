using AutoMapper;
using ProfileService.DAL.Models;
using ProfileService.BLL.Grpc.Services;
using Google.Protobuf.WellKnownTypes;
using ProfileService.BLL.Models;

namespace ProfileService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDataModel, UserProfile>()
                .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserProfile, ProtoProfileModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc))));

            CreateMap<UserProfile, UserProfileModel>().ReverseMap();
            CreateMap<UserProfile, UserProfileModelWithPrivacy>();
            CreateMap<ProfileInfoVisibility, ProfileInfoVisibilityModel>().ReverseMap();
        }
    }
}
