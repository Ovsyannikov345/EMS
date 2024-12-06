using AutoMapper;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Utilities.Pagination;
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

            CreateMap<EstateWithProfileModel, ProtoEstateModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))));
            CreateMap<Estate, ProtoEstateModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc))));
            CreateMap<UserProfileModel, ProtoProfileModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc))));

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
                .ForMember(nameof(PagedResult<object>.Results), opt => opt.MapFrom(nameof(PagedResult<object>.Results)));

            CreateMap<EstateFilterModel, EstateFilter>().ReverseMap();
        }
    }
}
