using AutoMapper;
using ChatService.BLL.Models;
using ChatService.DAL.Models.Entities;
using ProtoProfileModel = ChatService.DAL.Grpc.Services.Profile.ProtoProfileModel;
using ProtoEstateProfileModel = ChatService.DAL.Grpc.Services.Estate.ProtoProfileModel;

namespace ChatService.Tests.Mapping
{
    internal class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
            CreateMap<ChatModel, Chat>();
            CreateMap<ProtoProfileModel, ProtoEstateProfileModel>();
        }
    }
}
