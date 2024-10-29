using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.DAL.Grpc.Models;
using CatalogueService.DAL.Models.Entities;

namespace CatalogueService.Tests.Mapping
{
    internal class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
            CreateMap<UserProfileModel, UserProfile>();
            CreateMap<EstateWithProfileModel, Estate>();
        }
    }
}
