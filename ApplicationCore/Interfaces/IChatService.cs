using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IChatService
    {
        Task CreateChatAsync(string roomName);
        Task AddPersonToChat(string userGuid, int chatId);
        Task<IReadOnlyList<Chat>> GetChatsOnly();
        Task<IEnumerable<Chat>> GetChatsWithPeople();
        Task<Chat> GetChatDetails(int chatId);
    }
}
