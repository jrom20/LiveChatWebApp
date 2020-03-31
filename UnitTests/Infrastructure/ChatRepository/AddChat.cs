using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Infrastructure.ChatRepository
{
    public class AddChat
    {
        private readonly AppDataContext _applicationDbContext;

        public AddChat()
        {
            var options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _applicationDbContext = new AppDataContext(options);
            _applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task Should_InvokeFirstAsync_WithName_DefaultChat()
        {

            Chat resourceChat = new Chat()
            {
                RoomName = "Default Chat",
                StartDate = DateTime.Now
            };

            
            _applicationDbContext.Chats.Add(resourceChat);
            _applicationDbContext.SaveChanges();


            string chatName = (await _applicationDbContext.Chats.FirstAsync()).RoomName;
            Assert.Equal("Default Chat", chatName);
        }
    }
}
