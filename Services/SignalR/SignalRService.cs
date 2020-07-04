using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using System;
using NLog;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.SignalR
{
    public class SignalRService : ISignalRService
    {
        #region Fields

        private readonly ISignalRRepository _signalRRepository;
        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public SignalRService(ISignalRRepository signalRRepository)
        {
            _signalRRepository = signalRRepository;
        }

        #endregion

        #region Methods

        public async Task DeleteSignalRConnection(string connectionid)
        {
            var result=await _signalRRepository.DeleteAsync(connectionid);
            _logger.Debug($"SignalR: DeleteSignalRConnection; Result: {result}, Connection: {connectionid}");
        }


        public async Task SignalRUserConnected(string connectionid, string statusconnectionid, Guid userId)
        {
            var result = 0;
            var signalRconn = await _signalRRepository.FindConnectionByUserId(userId);
            _logger.Debug($"SignalR: SignalRUserConnected; Start: {connectionid}, {statusconnectionid}, {userId}, Connection: {signalRconn}");
            if (signalRconn == null)
            {
                signalRconn = new SignalRDto
                {
                    SignalRConnectionId = Guid.NewGuid(),
                    ConnectionID = connectionid,
                    Connected = true,
                    TimeStart = DateTime.Now,
                    TimeEnd = null,
                    UserId = userId,
                    StatusConnectionID = statusconnectionid
                };

                result = await _signalRRepository.InsertAsync(signalRconn);
                
            }
            else
            {
                if (!string.IsNullOrEmpty(connectionid) && string.IsNullOrEmpty(statusconnectionid))
                {
                    signalRconn.ConnectionID = connectionid;
                    result = await _signalRRepository.UpdateAsync(signalRconn);
                }
                else
                {
                    signalRconn.StatusConnectionID = statusconnectionid;
                    result = await _signalRRepository.UpdateAsync(signalRconn);
                }
            }
            _logger.Debug($"SignalR: SignalRUserConnected; Result: {result}, Connection: {signalRconn}");
        }

        async Task<SignalRDto> ISignalRService.GetConnectionByUserId(Guid userId)
        {
            var result= await _signalRRepository.FindConnectionByUserId(userId);
            _logger.Debug($"SignalR: SignalRUserConnected; Connection: {result}, User {userId}");
            return result;
        }

        #endregion
    }
}
