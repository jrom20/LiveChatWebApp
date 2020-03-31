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
    public class MessageService : IMessageService
    {
        private readonly IAsyncRepository<Message> _messagesRepository;
        private readonly IChatRepository _chatRepository;
        public MessageService(IAsyncRepository<Message> messagesRepository, IChatRepository chatRepository)
        {
            this._messagesRepository = messagesRepository;
            this._chatRepository = chatRepository;
        }

        public async Task AddNewMessage(int chatId, string userName, string message, DateTime time)
        {
            var existPersonInChat = (await _chatRepository.GetByIdWithItemsAsync(chatId)).People.FirstOrDefault(c=> c.ChatId == chatId && c.UserName == userName);
            if (existPersonInChat != null)
            {
                await _messagesRepository.AddAsync(new Message()
                {
                    CapturedDate = time,
                    ChatId = chatId,
                    TextMessage = message,
                    Person = existPersonInChat
                });
            }
        }

        public async Task<IEnumerable<Message>> GetChatMessagesById(int chatId)
        {
            var messagesList = await _messagesRepository.ListAsync(new MessageFilterPaginatedSpecification(0, 50, chatId));
            return messagesList;
        }
    }
}
