using AutoMapper;
using ChatService.BLL.Models;
using ChatService.ViewModels;

namespace ChatService.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ChatModel, ChatViewModel>();
            CreateMap<EstateModel, EstateViewModel>();
            CreateMap<UserProfileModel, UserProfileViewModel>();
            CreateMap<MessageModel, MessageViewModel>();
        }
    }
}
