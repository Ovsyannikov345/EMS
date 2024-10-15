using AutoMapper;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Models.Entities;
using Google.Protobuf.WellKnownTypes;

namespace CatalogueService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Estate, EstateWithProfileModel>();
            CreateMap<EstateModel, Estate>().ReverseMap();
            CreateMap<UserProfile, UserProfileModel>();

            CreateMap<EstateWithProfileModel, ProtoEstateModel>();
            CreateMap<UserProfileModel, ProtoProfileModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToTimestamp()));
        }
    }
}
