using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Chat;
using ApplicationCore.Entities;

namespace Web.ViewModels.Helpers
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            CreateMap<ApplicationCore.Entities.Chat, ChatViewModel>()
            .ForMember(o => o.ChatId, ex => ex.MapFrom(o => o.Id))
            .ForMember(o => o.ChatName, ex => ex.MapFrom(o => o.RoomName))
            .ReverseMap();

        }
    }
}
