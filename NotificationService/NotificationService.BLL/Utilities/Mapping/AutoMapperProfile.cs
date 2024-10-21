using AutoMapper;
using NotificationService.BLL.Models;
using NotificationService.DAL.Models.Entities;

namespace NotificationService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Notification, NotificationModel>().ReverseMap();
        }
    }
}
