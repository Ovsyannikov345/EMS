using AutoMapper;
using CatalogueService.BLL.Dto;
using CatalogueService.DAL.Models.Entities;

namespace CatalogueService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Estate, EstateFullDetails>();
        }
    }
}
