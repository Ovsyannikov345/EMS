﻿using AutoMapper;
using CatalogueService.DAL.Grpc.Services;
using ChatService.BLL.Models;
using ChatService.DAL.Models.Entities;

namespace ChatService.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProtoEstateModel, EstateModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Parse(src.Price)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<ProtoProfileModel, UserProfileModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime()));

            CreateMap<Chat, ChatModel>();
            CreateMap<Message, MessageModel>().ReverseMap();
        }
    }
}
