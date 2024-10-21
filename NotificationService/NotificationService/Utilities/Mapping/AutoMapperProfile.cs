using AutoMapper;
using NotificationService.BLL.Models;
using NotificationService.ViewModels;

namespace NotificationService.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NotificationModel, NotificationViewModel>();
            CreateMap<NotificationToCreateViewModel, NotificationModel>();
        }
    }
}
