using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Models.Entities;

namespace CatalogueService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Estate, EstateWithProfileModel>();
            CreateMap<EstateModel, Estate>().ReverseMap();
            CreateMap<UserProfile, UserProfileModel>();
        }
    }
}
