using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IAsyncRepository<Message> _messagesRepository;
        private readonly IAsyncRepository<Person> _personRepository;

        public ChatService(IChatRepository chatRepository,
            IAsyncRepository<Message> messagesRepository,
            IAsyncRepository<Person> personRepository)
        {
            this._chatRepository = chatRepository;
            this._messagesRepository = messagesRepository;
            this._personRepository = personRepository;
        }

        public async Task AddPersonToChat(string userGuid, int chatId)
        {
            await _personRepository.AddAsync(new Person()
            {
                IdentityGuid = userGuid,
                ChatId = chatId
            });
        }

        public async Task CreateChatAsync(string roomName)
        {
            await _chatRepository.AddAsync(new Chat()
            {
                RoomName = roomName,
                StartDate = DateTime.Now
            });
        }

        public async Task<IEnumerable<Message>> GetChatMessagesById(int chatId)
        {
            var messagesList = await _messagesRepository.ListAsync(new MessageFilterPaginatedSpecification(0, 50, chatId));
            return messagesList;
        }

        public async Task<IReadOnlyList<Chat>> GetChatsOnly()
        {
            var chatsList = await _chatRepository.ListAllAsync();
            return chatsList;
        }

        public async Task<Chat> GetChatDetails(int chatId)
        {
            var chatsList = await _chatRepository.GetByIdWithItemsAsync(chatId);
            return chatsList;
        }

        public Task<IEnumerable<Chat>> GetChatsWithPeople()
        {
            return _chatRepository.GetAllChatWithPeopleAsync();
        }
    }
}
