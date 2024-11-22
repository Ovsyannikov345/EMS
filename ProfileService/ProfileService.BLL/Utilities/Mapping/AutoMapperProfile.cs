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
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc))))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc))));

            CreateMap<UserProfile, UserProfileModel>().ReverseMap();
            CreateMap<UserProfile, UserProfileModelWithPrivacy>();
            CreateMap<ProfileInfoVisibility, ProfileInfoVisibilityModel>().ReverseMap();

            CreateMap<UserProfileModel, ProtoProfileModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc))))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc))));
            CreateMap<UserProfileModelWithPrivacy, ProtoProfileModel>()
                .ForMember(dest => dest.BirthDate, opt =>
                {
                    opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate.GetValueOrDefault(), DateTimeKind.Utc)));
                    opt.Condition(src => src.BirthDate is not null);
                })
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc))))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.BirthDate is not null));
        }
    }
}
