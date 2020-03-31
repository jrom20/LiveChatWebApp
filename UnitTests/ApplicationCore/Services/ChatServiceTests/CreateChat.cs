using ApplicationCore.Interfaces;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ApplicationCore.Services.ChatServiceTests
{
    public class CreateChat
    {
        private readonly Mock<IChatRepository> _mockChatRepo;
        private readonly Mock<IAsyncRepository<Person>> _mockPersonRepo;

        public CreateChat()
        {
            _mockChatRepo = new Mock<IChatRepository>();
            _mockPersonRepo = new Mock<IAsyncRepository<Person>>();
        }

        [Fact]
        public async Task Should_InvokeGetChatsWithPeople_Once()
        {
            //Arrange
            var chatService = new ChatService(_mockChatRepo.Object, _mockPersonRepo.Object);

            //Act
            await chatService.CreateChatAsync("Default Room");

            //Assert
            _mockChatRepo.Verify(x => x.AddAsync(It.IsAny<Chat>()), Times.Once);
        }

    }
}
