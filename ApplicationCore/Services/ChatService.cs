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
        private readonly IAsyncRepository<Person> _personRepository;

        public ChatService(IChatRepository chatRepository,
            IAsyncRepository<Person> personRepository)
        {
            this._chatRepository = chatRepository;
            this._personRepository = personRepository;
        }

        public async Task AddPersonToChat(string userGuid, int chatId)
        {
            var existPersonInChat = (await _chatRepository.GetByIdWithItemsAsync(chatId)).People.FirstOrDefault(c => c.ChatId == chatId && c.IdentityGuid == userGuid);
            if (existPersonInChat == null)
            {
                await _personRepository.AddAsync(new Person()
                {
                    IdentityGuid = userGuid,
                    ChatId = chatId
                });
            }
        }

        public async Task CreateChatAsync(string roomName)
        {
            await _chatRepository.AddAsync(new Chat()
            {
                RoomName = roomName,
                StartDate = DateTime.Now
            });
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
