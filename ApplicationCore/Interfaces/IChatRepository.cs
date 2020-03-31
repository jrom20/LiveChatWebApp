using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IChatRepository : IAsyncRepository<Chat>
    {
        Task<Chat> GetByIdWithItemsAsync(int id);
        Task<IEnumerable<Chat>> GetAllChatWithPeopleAsync();
    }
}
