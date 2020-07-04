using ChatBotInt.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Chat
{
    public interface IChatService
    {
        public Task<int> SendMessageFromChat(ChatMessageDTO message);
        public Task<int> SendMessageToChat(SendMessageModel message);

        #region Get message
        //Get all message by id sessions
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesBySessionId(Guid sessionId);
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByGroupId(Guid groupId);
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByUserId(Guid userId);

        #endregion

        #region Get Users

        Task<List<Guid>> GetUsersBySessionId(Guid sessionId);
        Task<List<Guid>> GetUsersByGroupId(Guid groupId);

        #endregion
        //Task<int> SaveMessageToDB(ChatMessageDTO model);
        //Task<int> AddSendMessages(SendMessageModel message);
    }
}
