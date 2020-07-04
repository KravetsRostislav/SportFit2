using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatBotInt.Repositories.Models;

namespace Services.SignalR
{
    public interface ISignalRService
    {
        Task DeleteSignalRConnection(string connectionid);
        Task SignalRUserConnected(string connectionid, string statusconnectionid, Guid operatorid);
        Task<ChatBotInt.Repositories.Models.SignalRDto> GetConnectionByUserId(Guid userId);
    }

}
