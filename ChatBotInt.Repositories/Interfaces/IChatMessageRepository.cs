using ChatBotInt.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories.Interfaces
{
    public interface IChatMessageRepository
    {
        #region Get Messages

        //Get all message by id sessions
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesBySessionId(Guid sessionId);
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByGroupId(Guid groupId);
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByUserId(Guid userId);

        #endregion

        #region Get Users

        Task<List<Guid>> GetUsersBySessionId(Guid sessionId);
        Task<IEnumerable<Guid>> GetUsersByGroupId(Guid groupId);
        #endregion

        Task<int> SaveMessageToDB(ChatMessageDTO model);
        Task<int> AddSendMessages(SendMessageModel message);
        //upd?

    }
}
