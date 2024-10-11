using AutoMapper;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Grpc.Services;

namespace CatalogueService.DAL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProtoProfileModel, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForCtorParam(nameof(UserProfile.BirthDate), opt => opt.MapFrom(src => src.BirthDate.ToDateTime()));
        }
    }
}
