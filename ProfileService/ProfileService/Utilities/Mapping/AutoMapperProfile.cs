using AutoMapper;
using ProfileService.BLL.Models;
using ProfileService.ViewModels;

namespace ProfileService.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDataViewModel, RegistrationDataModel>();
            CreateMap<UserProfileModel, UserProfileViewModel>();
        }
    }
}
