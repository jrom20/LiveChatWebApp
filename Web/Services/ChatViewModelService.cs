using ApplicationCore.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels.Chat;

namespace Web.Services
{
    public class ChatViewModelService : IChatViewModelService
    {
        private readonly IChatService _chatService;
        public ChatViewModelService(IChatService chatService)
        {
            this._chatService = chatService;
        }

        public async Task CreateANewRoomAsync(string roomName)
        {
            await _chatService.CreateChatAsync(roomName);
        }

        public async Task<IEnumerable<ChatViewModel>> GetChatsWithTotalPeopleAsync()
        {
            var chatList = new List<ChatViewModel>();
            var chatListRead = await _chatService.GetChatsWithPeople();
            foreach (var chatItem in chatListRead)
                chatList.Add(new ChatViewModel()
                {
                    ChatId = chatItem.Id,
                    ChatName = chatItem.RoomName,
                    PeopleNumber = chatItem.People == null ? 0 : chatItem.People.Count,
                    StartedDate = chatItem.StartDate
                });
            return chatList;
        }

    }
}
