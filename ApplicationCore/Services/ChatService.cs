using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public async Task AddPersonToChat(string userGuid, int chatId)
        {
            var chat = await chatRepository.GetByIdAsync(chatId);
        }

        public Task CreateChatAsync(string roomName)
        {
            throw new NotImplementedException();
        }

        public Task GetChatMessagesById(int chatId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Chat>> GetChats()
        {
            var chatsList = await chatRepository.ListAllAsync();
            return chatsList;
        }
    }
}
