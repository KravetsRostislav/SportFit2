using ChatBotInt.Repositories.Interfaces;
using Moq;
using SoftPhone.M.ChatBotInt.UnitTests.Chat;
using System.Linq;
using Xunit;

namespace SoftPhone.M.ChatBotIntTests.Chat
{
    public class ChatMessageRepositoryTests
    {
        [Fact]
        public void AddSendMessagesTest()
        {
            var message = new ServiceFake().ListSendMessages().FirstOrDefault();

            var mockpersonRepository = new Mock<IChatMessageRepository>();
            mockpersonRepository.Setup(x => x.AddSendMessages(message));
            mockpersonRepository.Object.AddSendMessages(message);
            mockpersonRepository.Verify(x => x.AddSendMessages(message), Times.Once()); //Assert that the Add method was called once

        }
    }
    
}
