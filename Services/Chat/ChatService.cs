using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Chat
{
    public class ChatService: IChatService
    {
        #region Fields

        private readonly IChatMessageRepository _chatRepository;
        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public ChatService(IChatMessageRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        #endregion

        public async Task<int> SendMessageFromChat(ChatMessageDTO message)
        {
            _logger.Info($"{"ChatService:",-20} >>> {"SendMessageFromChat",-20} >>> {"Start model:",-10} {JsonConvert.SerializeObject(message)}.");
            if (message == null)
                return 0;

            var res = await _chatRepository.SaveMessageToDB(message);

            _logger.Debug($"{"ChatService:",-20} >>> {"SendMessageFromChat",-20} >>> {"Save:",-10} {res}.");
            return res;
            //if (res.Result != 0)
            //{
            //    var clientModel = new ChatMessageDTO
            //    {
            //        SessionId = message.SessionId,
            //        ChatId = message.ChatId,
            //        MessageDateTime = message.MessageDateTime,
            //        MessageContent = "testContent",
            //        IsRead = message.IsRead,
            //        SenderUserId = message.SenderUserId,
            //        IsGroup = message.IsGroup,
            //        GroupId= message.GroupId
            //    };
            //}
        }

        public async Task<int> SendMessageToChat(SendMessageModel message)
        {
            int res = 0;
            _logger.Info($"{"ChatService:",-20} >>> {"SendMessageToChat",-20} >>> {"Start model:",-10} {JsonConvert.SerializeObject(message)}.");

            if (message == null)
                return 0;

            res = await _chatRepository.AddSendMessages(message);
            _logger.Debug($"{"ChatService:",-20} >>> {"SendMessageToChat",-20} >>> {"Model:",-10} {JsonConvert.SerializeObject(message)}, {"Send:",-10}{res}.");;
            return res;
            
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByGroupId(Guid groupId)
        {
            _logger.Info(String.Format($"{"ChatService",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"Start",-10} >>> {"GroupId",-10}{groupId}"));
            IEnumerable<ChatMessageDTO> result = null;
            
            if (groupId == null || groupId == Guid.Empty)
                return result;

            result = await _chatRepository.GetAllMessagesByGroupId(groupId);
            
            _logger.Debug($"{"ChatService:",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"Result:",-10} {JsonConvert.SerializeObject(result)}, {"GroupId:",-10}{groupId}.");

            return result;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesBySessionId(Guid sessionId)
        {
            _logger.Info(String.Format($"{"ChatService",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"Start",-10} >>> {"SessionId",-10}{sessionId}"));
            IEnumerable<ChatMessageDTO> result = null;

            //if (sessionId == null || sessionId == Guid.Empty)
            //    return result;

            result = await _chatRepository.GetAllMessagesBySessionId(sessionId);

            _logger.Debug($"{"ChatService:",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"Result:",-10} {JsonConvert.SerializeObject(result)}, {"SessionId:",-10}{sessionId}.");

            return result;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesByUserId(Guid userId)
        {
            _logger.Info(String.Format($"{"ChatService",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Start",-10} >>> {"UserId",-10}{userId}"));
            IEnumerable<ChatMessageDTO> result = null;

            if (userId == null || userId == Guid.Empty)
                return result;

            result = await _chatRepository.GetAllMessagesByUserId(userId);

            _logger.Debug($"{"ChatService:",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Result:",-10} {JsonConvert.SerializeObject(result)}, {"UserId:",-10}{userId}.");

            return result;
        }

        public async Task<List<Guid>> GetUsersByGroupId(Guid groupId)
        {
            _logger.Info(String.Format($"{"ChatService",-20} >>> {"GetUsersByGroupId",-20} >>> {"Start",-10} >>> {"GroupId",-10}{groupId}"));
            List<Guid> result = new List<Guid>();

            if (groupId == null || groupId == Guid.Empty)
                return result;

            var results = await _chatRepository.GetUsersByGroupId(groupId);

            _logger.Debug($"{"ChatService:",-20} >>> {"GetUsersByGroupId",-20} >>> {"Result:",-10} {JsonConvert.SerializeObject(result)}, {"GroupId:",-10}{groupId}.");

            return result.ToList();
        }

        public async Task<List<Guid>> GetUsersBySessionId(Guid sessionId)
        {
            _logger.Info(String.Format($"{"ChatService",-20} >>> {"GetUsersBySessionId",-20} >>> {"Start",-10} >>> {"SessionId",-10}{sessionId}"));
            List<Guid> result = new List<Guid>();

            if (sessionId == null || sessionId == Guid.Empty)
                return result;

            result = await _chatRepository.GetUsersBySessionId(sessionId);

            _logger.Debug($"{"ChatService:",-20} >>> {"GetUsersBySessionId",-20} >>> {"Result:",-10} {JsonConvert.SerializeObject(result)}, {"SessionId:",-10}{sessionId}.");

            return result;
        }
    }
}
