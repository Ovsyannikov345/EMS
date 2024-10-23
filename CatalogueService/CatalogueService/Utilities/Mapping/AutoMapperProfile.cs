using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.ViewModels;

namespace CatalogueService.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EstateToCreateViewModel, EstateModel>();
            CreateMap<EstateToUpdateViewModel, EstateModel>();
            CreateMap<EstateModel, EstateViewModel>();
            CreateMap<EstateWithProfileModel, EstateWithProfileViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<UserProfileModel, UserProfileViewModel>();
            CreateMap<EstateFilterModel, EstateFilterViewModel>().ReverseMap();
            CreateMap<EstateFilterToCreateViewModel, EstateFilterModel>();
        }
    }
}
