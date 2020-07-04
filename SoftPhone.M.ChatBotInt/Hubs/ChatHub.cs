using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NLog;
using Services.SignalR;
using SoftPhone.M.ChatBotInt.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Hubs
{
    public class ChatHub: Hub
    {
        #region Fields
        
        private readonly ISignalRService _signalRService;
        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public ChatHub(ISignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        #endregion

        #region Methods

        public async Task UserConnected(Guid userId)
        {
            _logger.Info($"{"ChatHub:",-20} >>> {"UserConnected",-20} >>> {"Start: UserId:",-10} {userId}.");
            
            await _signalRService.SignalRUserConnected(Context.ConnectionId, null, userId);
            _logger.Debug($"{"ChatHub:",-20} >>> {"UserConnected",-20} >>> {"Connection:",-10} {JsonConvert.SerializeObject(Context.ConnectionId)}.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.Info($"{"ChatHub:",-20} >>> {"OnDisconnectedAsync",-20} >>> {"Start: Connection:",-10} {JsonConvert.SerializeObject(Context.ConnectionId)}.");
           
            await _signalRService.DeleteSignalRConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToClients(string message)
        {
            //todo  savetodb
            await Clients.All.SendAsync(message);
        }

        

        #endregion

    }
}
