using AutoMapper;
using CatalogueService.BLL.Grpc.Models;
using CatalogueService.BLL.Grpc.Services;

namespace CatalogueService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProtoProfileModel, UserProfile>();
        }
    }
}
