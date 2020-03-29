using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDataContextSeed
    {
        private readonly AppDataContext _ctxData;

        public AppDataContextSeed(AppDataContext ctxData)
        {
            _ctxData = ctxData;
        }
        public void SeedAsync()
        {
            _ctxData.Database.EnsureCreated();

            if(!_ctxData.Chats.Any())
            {
                Chat chat = new Chat();
                chat.StartDate = DateTime.Now;
                chat.RoomName = "Default Room";

                _ctxData.Chats.Add(chat);
            }

            _ctxData.SaveChanges();
        }
    }
}
