using ChatBotInt.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftPhone.M.ChatBotInt.UnitTests.Chat
{
    public class ServiceFake
    {
        public IEnumerable<ChatMessageDTO> ListChatMessages()
        {
            List<ChatMessageDTO> messages = new List<ChatMessageDTO>()
            {
                new ChatMessageDTO
                {
                    ChatId=1,
                    SessionId=Guid.Parse("94D2EC66-5DA2-4023-9289-A47E19510C8C"),
                    SenderUserId=Guid.Parse("3dd0141a-60b6-4dc9-b3f8-4237c4748af9"),
                    MessageContent="Test content",
                    MessageDateTime=DateTime.Now,
                    GroupId=Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    IsGroup=true,
                    IsRead=true
                },
                new ChatMessageDTO
                {
                    ChatId=1412,
                    SessionId=Guid.Parse("C882796E-08B9-475A-A35B-3DFF67A73FFA"),
                    SenderUserId=Guid.Parse("05213a89-95bd-43e4-bc6a-541672b84126"),
                    MessageContent="Test content 2",
                    MessageDateTime=DateTime.Now,
                    GroupId=Guid.Parse("8C4D3065-5CE4-48AF-B35A-074702C9A3EE"),
                    IsGroup=false,
                    IsRead=true
                },
            };

            return messages;
        }
        public IEnumerable<SendMessageModel> ListSendMessages()
        {
            List<SendMessageModel> messages = new List<SendMessageModel>()
            {
                new SendMessageModel
                {
                    SessionId=Guid.Parse("94D2EC66-5DA2-4023-9289-A47E19510C8C"),
                    MessageText="Test content",
                    DateTime=DateTime.Now,
                    UserId=Guid.Parse("05213a89-95bd-43e4-bc6a-541672b84126"),
                    UserEmail="",
                },
                new SendMessageModel
                {
                    SessionId=Guid.Parse("C882796E-08B9-475A-A35B-3DFF67A73FFA"),
                    MessageText="Test content 2",
                    DateTime=DateTime.Now,
                    UserId=Guid.Parse("3dd0141a-60b6-4dc9-b3f8-4237c4748af9"),
                    UserEmail=""
                },
            };

            return messages;
        }

        
    }
}
