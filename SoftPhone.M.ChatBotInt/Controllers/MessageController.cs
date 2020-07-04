using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using SoftPhone.M.ChatBotInt.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Fields

        private readonly IMessageRepository _messageRepository;
        Logger _logger = LogManager.GetCurrentClassLogger();
        private IProducerService _producer;

        #endregion

        #region Ctor

        public MessageController(IMessageRepository messageRepository, IProducerService producerService)
        {
            _messageRepository = messageRepository;
            _producer = producerService;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Закрити сесію на стороні чат-бота
        /// </summary>
        /// <param name="sessionid">Ідентифікатор сесії</param>
        /// <remarks>
        /// true - успішне виконання процедури
        /// false - не успішне виконання процедури
        /// </remarks>        
        [HttpGet("OperatorCloseSession")]
        public async Task<bool> OperatorCloseSession(Guid sessionid)
        {            
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"OperatorCloseSession",-20} >>> {"Start: SessionId:",-10} {sessionid}.");
                bool res = await _messageRepository.OperatorCloseSession(sessionid);
                _logger.Debug($"{"MessageController:",-20} >>> {"OperatorCloseSession",-20} >>> {"Responce:",-10} {res}.");
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return false;
            }
        }

        /// <summary>
        /// Відправка повідомлення від оператора до клієнта
        /// </summary>
        /// <param name="sendMessage">Модель для відправки повідомлення до клієнта</param>
        /// <remarks>
        /// При успішному відправленні отримуємо повідомлення, інакше виникає виключення
        /// </remarks> 
        [HttpPost("send-message")]
        public async Task<IActionResult> SetMessageToDB(SendMessageModel sendMessage)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"SetMessageToDB",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(sendMessage)}.");
                string result = await _messageRepository.SetMessageToDB(sendMessage);
                _logger.Debug($"{"MessageController:",-20} >>> {"SetMessageToDB",-20} >>> {"SessionId:",-10}{sendMessage.SessionId, -20}{"Response:", 10}{result}.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Перевести клієнта назад в ITR
        /// </summary>
        /// <param name="iTRDto">Модель для повернення клієнта до гілки ITR</param>
        /// <returns></returns>
        /// <remarks>Метод повертає клієнта в вітку ITR, яку визначив оператор</remarks>
        [HttpPost("ReturnBrancheToITR")]
        public async Task<IActionResult> ReturnBrancheToITR([FromBody] ReturnBrancheITRModel iTRDto)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"ReturnBrancheToITR",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(iTRDto)}.");
                bool result = await _messageRepository.ReturnBrancheToITR(iTRDto);
                
                _logger.Debug($"{"MessageController:",-20} >>> {"ReturnBrancheToITR",-20} >>> {"SessionId:",-10}{iTRDto.SessionId,-20}{"Response:",10}{result}.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Збереження теми, по якій клієнт звернувся з питанням
        /// </summary>
        /// <param name="model">Модель для збереження теми звернення</param>
        /// <returns></returns>
        [HttpPost("SaveThemeOfTreatment")]
        public async Task<IActionResult> SaveThemeOfTreatment([FromBody] ThemeOfTreatmentModel model)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"SaveThemeOfTreatment",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(model)}.");
                bool result = await _messageRepository.SaveThemeOfTreatment(model);

                _logger.Debug($"{"MessageController:",-20} >>> {"SaveThemeOfTreatment",-20} >>> {"SessionId:",-10}{model.SessionId,-20}{"Response:",10}{result}.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        #endregion

        #region Methods old

        /// <summary>
        /// Отримати всі повідомлення по id сесії
        /// </summary>
        /// <param name="sessionid">Ідентифікатор сесії</param>
        /// <remarks>
        /// Повертається масив всіх повідомлень які відповідають сесії
        /// </remarks>
        /// <returns></returns>
        [HttpGet("get-all-messages-by-session-id")]
        public async Task<IActionResult> GetAllMessagesBySessionId(Guid sessionid)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionid}.");
                IEnumerable<ChatMessageDto> messages = await _messageRepository.GetAllMessagesBySessionId(sessionid);

                _logger.Debug($"{"MessageController:",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"SessionId:",-10}{sessionid,-20}{"Message:",20}{messages.ToList().Count(),10}.");
                return Ok(messages);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Отримати всі остані активні сесії по їх id
        /// </summary>
        /// <param name="sessionId">Ідентифікатор сесії</param>
        /// <remarks>
        /// Повертається масив всіх останії сессії
        /// </remarks>
        [HttpPost("Get-last-messages-by-sessions")]
        public async Task<IActionResult> GetLastMessagesBySessions([FromBody] PassSessionId sessionId)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"GetLastMessagesBySessions",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(sessionId)}.");
                IEnumerable<MessageForSessionModel> messages = await _messageRepository.GetLastMessages(JsonConvert.SerializeObject(sessionId));

                _logger.Debug($"{"MessageController:",-20} >>> {"GetLastMessagesBySessions",-20} >>> {"Model:",-10}{JsonConvert.SerializeObject(sessionId),-20}{"Message:",20}{messages.ToList().Count(),10}.");
                return Ok(messages);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Отримати всі повідомлення по оператору
        /// </summary>
        /// <param name="userId">Ідентифікатор оператора</param>
        /// <remarks>
        /// Повертається масив всіх повідомлень по оператору
        /// </remarks>
        [HttpPost("GetMonitoringChatsByUserId")]
        public async Task<IActionResult> GetMonitoringChatsByUserId([FromBody] Guid userId)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"GetMonitoringChatsByUserId",-20} >>> {"Start: UserId:",-10} {userId}.");
                IEnumerable<MonitoringChats> messages = await _messageRepository.GetMonitoringChatsByUserId(userId);
                _logger.Debug($"{"MessageController:",-20} >>> {"GetMonitoringChatsByUserId",-20} >>> {"UserId:",-10}{userId,-20}{"Message:",20}{messages.ToList().Count(),10}.");
                return Ok(messages);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Отримати інформацію по клієнту
        /// </summary>
        /// <param name="sessionId">Ідентифікатор сесії</param>
        /// <returns></returns>
        [HttpGet("get-client-by-session-id")]
        public async Task<IActionResult> GetClientBySessionId(Guid sessionId)
        {
            try
            {
                _logger.Info($"{"MessageController:",-20} >>> {"GetClientBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionId}.");
                CrmClientDto client = await _messageRepository.GetClientBySessionId(sessionId);

                _logger.Debug($"{"MessageController:",-20} >>> {"GetClientBySessionId",-20} >>> {"SessionId:",-10} {sessionId,-20} Response: {JsonConvert.SerializeObject(client),10}.");
                return Ok(client);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("KafkaTestMessage")]
        public async Task<IActionResult> KafkaTestMessage([FromBody]SendMessageModel messageModel)
        {
            _logger.Info($"{"MessageController:",-20} >>> {"KafkaTestMessage",-20} >>> {"Input Message:",-10} {JsonConvert.SerializeObject(messageModel)}.");
            var message = JsonConvert.SerializeObject(messageModel);
            
            var result = await _producer.Produce(new Message<Null, string> { Value = message });
            _logger.Debug($"Message: {JsonConvert.SerializeObject(result),20} >>> {"Result",10}{JsonConvert.SerializeObject(result), 20}");
            return Ok(result);
        }

        #endregion
    }
}