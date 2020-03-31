using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetChatMessagesById(int chatId);
        Task AddNewMessage(int chatId, string userName, string message, DateTime time);
    }
}
