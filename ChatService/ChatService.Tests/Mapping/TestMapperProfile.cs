using AutoMapper;
using ChatService.BLL.Models;
using ChatService.DAL.Models.Entities;

namespace ChatService.Tests.Mapping
{
    internal class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
            CreateMap<ChatModel, Chat>();
            CreateMap<DAL.Grpc.Services.Profile.ProtoProfileModel, DAL.Grpc.Services.Estate.ProtoProfileModel>();
        }
    }
}
