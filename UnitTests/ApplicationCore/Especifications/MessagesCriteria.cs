using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ApplicationCore.Especifications
{
    public class MessagesCriteria
    {

        public MessagesCriteria()
        {

        }

        [Fact]
        public void Should_MatchesMessagesWithGivenId()
        {
            //Arrange
            int chatId = 1;
            var spec = new MessageFilterPaginatedSpecification(0, 50, chatId);

            //Act
            var result = GetTestMessageCollection().AsQueryable().Where(spec.Criteria);

            //Assert
            Assert.True(result.Count() == 80);
        }


        public List<Message> GetTestMessageCollection()
        {
            List<Message> messagesList = new List<Message>();
            for (int counter = 1; counter <= 80; counter++)
            {
                var message = new Mock<Message>(1, DateTime.Now);
                message.SetupGet(s => s.Id).Returns(counter);
                messagesList.Add(message.Object);
            }
            return messagesList;
        }
    }
}
