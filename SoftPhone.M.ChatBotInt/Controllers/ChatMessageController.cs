using ChatBotInt.Repositories.Interfaces;
using ChatBotInt.Repositories.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NLog;
using Services.Chat;
using SoftPhone.M.ChatBotInt.Hubs;
using SoftPhone.M.ChatBotInt.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatMessageController: ControllerBase
	{
		#region Fields

		private readonly IChatMessageRepository _chatHttpClient;
		private IHubContext<ChatHub> _chatHubContext;
		private IChatService _chatService;
		private IProducerService _producer;

		private bool _kafkaIntegration;
		Logger _logger = LogManager.GetCurrentClassLogger();

		#endregion

		#region Ctor

		public ChatMessageController(IChatService chatService, IProducerService producerService, IChatMessageRepository chatHttpClient)
		{
			_chatService = chatService;
			_producer = producerService;
			_chatHttpClient = chatHttpClient;
		}

		#endregion

		#region Methods

		[HttpGet("GetAllMessagesBySessionId")]
		public async Task<IActionResult> GetAllMessagesBySessionId(Guid sessionId)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionId}.");

			IEnumerable<ChatMessageDTO> messages = await _chatService.GetAllMessagesBySessionId(sessionId);
			
			_logger.Debug($"{"ChatMessageController:",-20} >>> {"GetAllMessagesBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionId,-20} >>> {"Response:",-10} {messages.ToList().Count()}.");
			return Ok(messages);
		}

		[HttpGet("GetAllMessagesByGroupId")]
		public async Task<IActionResult> GetAllMessagesByGroupId(Guid groupId)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"Start: GroupId:",-10} {groupId}.");

			IEnumerable<ChatMessageDTO> messages = await _chatService.GetAllMessagesByGroupId(groupId);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"GetAllMessagesByGroupId",-20} >>> {"Start: GroupId:",-10} {groupId,-20} >>> {"Response:",-10} {messages.ToList().Count()}.");
			return Ok(messages);
		}

		[HttpGet("GetAllMessagesByUserId")]
		public async Task<IActionResult> GetAllMessagesByUserId(Guid userId)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Start: UserId:",-10} {userId}.");
			
			IEnumerable<ChatMessageDTO> messages = await _chatService.GetAllMessagesByUserId(userId);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"GetAllMessagesByUserId",-20} >>> {"Start: UserId:",-10} {userId,-20}>>> {"Messages:",-10} {messages.ToList().Count()}.");
			return Ok(messages);
		}

		[HttpGet("GetUsersBySessionId")]
		public async Task<IActionResult> GetUsersBySessionId(Guid sessionId)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"GetUsersBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionId}.");
			
			List<Guid> users = await _chatService.GetUsersBySessionId(sessionId);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"GetUsersBySessionId",-20} >>> {"Start: SessionId:",-10} {sessionId,-20} >>> {"Users:",-10} {users.Count()}.");
			return Ok(users);
		}

		[HttpGet("GetUsersByGroupId")]
		public async Task<IActionResult> GetUsersByGroupId(Guid groupId)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"GetUsersByGroupId",-20} >>> {"Start: GroupId:",-10} {groupId}.");
			
			List<Guid> users = await _chatService.GetUsersByGroupId(groupId);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"GetUsersByGroupId",-20} >>> {"Start: GroupId:",-10} {groupId,-20} >>> {"Users:",-10} {users.Count()}.");
			return Ok(users);
		}
		/// <summary>
		/// Save message to DB
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("SaveMessageToDB")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> SaveMessageToDB(ChatMessageDTO model)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"SaveMessageToDB",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(model)}.");

			//if (model is null || !ModelState.IsValid)
			//{
			//	_logger.Debug($"{"ChatMessageController:",-20} >>> {"SaveMessageToDB",-20} >>> {"Start: ModelState is valid:",-10} {ModelState.IsValid,-20} >>> {"Model is null:",-10}{model.Equals(null)}.");
			//	return BadRequest();
			//}

			var result = await _chatHttpClient.SaveMessageToDB(model);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"SaveMessageToDB",-20} >>> {"Start: model:",-10} {JsonConvert.SerializeObject(model),-20} >>> {"Responce",-10}{result}.");
			return Ok(result);

		}

		/// <summary>
		/// Sent message from chat
		/// </summary>
		[HttpPost("SendMessageFromChat")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> SendMessageFromChat(ChatMessageDTO message)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"SendMessageFromChat",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(message)}.");

			var connectionId = "";
			//if (message is null || !ModelState.IsValid)
			//{
			//	_logger.Debug($"{"ChatMessageController:",-20} >>> {"SendMessageFromChat",-20} >>> {"Start: ModelState is valid:",-10} {ModelState.IsValid,-20} >>> {"Model is null:",-10}{message.Equals(null)}.");
			//	return BadRequest();
			//}
			
			var sendResult = await _chatService.SendMessageFromChat(message);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"SendMessageFromChat",-20} >>> {"Start: model:",-10} {JsonConvert.SerializeObject(message),-20} >>> {"Responce:",-10}{sendResult}.");
			if (sendResult > 0)
			{
				if (connectionId != null)
				{
					await _chatHubContext.Clients.Client(connectionId).SendAsync("SendMessageFromChat", message);
				}
				_logger.Info($"SEND SUCCESS.");
				return Ok(sendResult);
			}

			return BadRequest();
		}

		[HttpPost("SendMessageToChat")]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> SendMessageToChat(SendMessageModel model)
		{
			_logger.Info($"{"ChatMessageController:",-20} >>> {"SendMessageToChat",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(model)}.");
			var connectionId = "";
			//if (model is null || !ModelState.IsValid)
			//{
			//	_logger.Debug($"{"ChatMessageController:",-20} >>> {"SendMessageToChat",-20} >>> {"Start: ModelState is valid:",-10} {ModelState.IsValid,-20} >>> {"Model is null:",-10}{model.Equals(null)}.");
			//	return BadRequest();
			//}

			var res = await _chatService.SendMessageToChat(model);

			_logger.Debug($"{"ChatMessageController:",-20} >>> {"SendMessageToChat",-20} >>> {"Start: Model:",-10} {JsonConvert.SerializeObject(model),-20} >>> {"Responce:",-10}{res}.");

			if (connectionId != null)
			{
				await _chatHubContext.Clients.Client(connectionId).SendAsync("SendMessageToChat", model);
			}
			_logger.Info($"SEND SUCCESS.");
			return Ok(res);
		}
		#endregion
	}
}
