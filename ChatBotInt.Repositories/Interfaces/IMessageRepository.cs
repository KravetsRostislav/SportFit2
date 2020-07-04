using ChatBotInt.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<ChatMessageDto>> GetAllMessagesBySessionId(Guid sessionid);
        Task<IEnumerable<GetAllMessagesDto>> GetAllMessages();
        Task<IEnumerable<MessageForSessionModel>> GetLastMessages(string sessionsId);
        Task<IEnumerable<MonitoringChats>> GetMonitoringChatsByUserId(Guid userId);
        Task<bool> OperatorCloseSession(Guid sessionid);
        Task<string> SetMessageToDB(SendMessageModel sendMessage);
        Task<bool> SaveThemeOfTreatment(ThemeOfTreatmentModel model);
        Task<bool> ReturnBrancheToITR(ReturnBrancheITRModel iTRDto);
        //Task<bool> ReturnBrancheToITR(SoftPhone.Models.ChatBot.ReturnBrancheITRModel iTRDto);
        Task<CrmClientDto> GetClientBySessionId(Guid sessionId);
    }
}
