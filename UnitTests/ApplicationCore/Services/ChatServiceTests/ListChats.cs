using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ApplicationCore.Services.ChatServiceTests
{
    public class ListChats
    {
        private readonly Mock<IChatRepository> _mockChatRepo;
        private readonly Mock<IAsyncRepository<Person>> _mockPersonRepo;

        public ListChats()
        {
            _mockChatRepo = new Mock<IChatRepository>();
            _mockPersonRepo = new Mock<IAsyncRepository<Person>>();
        }

        [Fact]
        public async Task Should_InvokeChatRepositoryGetAllChatWithPeopleAsync_SizeOfOne()
        {
            //Arrange

            List<Chat> chats = new List<Chat>();
            var chat = new Mock<Chat>();
            chat.SetupGet(s => s.Id).Returns(1);
            chats.Add(chat.Object);
            
            var chatService = new ChatService(_mockChatRepo.Object, _mockPersonRepo.Object);
            _mockChatRepo.Setup(x => x.GetAllChatWithPeopleAsync())
                .ReturnsAsync(chats);
            //Act
            var result = (await chatService.GetChatsWithPeople()).ToList();

            //Assert
            _mockChatRepo.Verify(x => x.GetAllChatWithPeopleAsync(), Times.Once);
            Assert.Single(result);
        }
    }
}
