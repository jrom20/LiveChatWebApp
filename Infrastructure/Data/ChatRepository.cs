using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ChatRepository: EfRepository<Chat>, IChatRepository
    {
        public ChatRepository(AppDataContext dbContext): base(dbContext)
        {

        }
        
        public Task<Chat> GetByIdWithItemsAsync(int id)
        {
            return _dbContext.Chats
                .Include(chat => chat.People)
                .Include($"{nameof(Chat.People)}.{nameof(ChatItem.Person)}")
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
