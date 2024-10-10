using AutoMapper;
using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Grpc.Models;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.DAL.Models.Entities;

namespace CatalogueService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProtoProfileModel, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForCtorParam("BirthDate", opt => opt.MapFrom(src => src.BirthDate.ToDateTime()));
            CreateMap<Estate, EstateFullDetails>();
        }
    }
}
