using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Chat;

namespace Web.Interfaces
{
    public interface IChatViewModelService
    {
        Task<IEnumerable<ChatViewModel>> GetChatsWithTotalPeopleAsync();
        Task JoinPersonToRoomAsync(string userName, string chatId);
        Task CreateANewRoomAsync(string roomName);
    }
}
