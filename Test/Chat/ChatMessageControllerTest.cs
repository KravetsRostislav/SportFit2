using System;
using Xunit;
using Services.Chat;
using SoftPhone.M.ChatBotInt.Controllers;
using Moq;
using ChatBotInt.Repositories.Models;
using ChatBotInt.Repositories.Interfaces;
using System.Linq;
using NLog;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit.Sdk;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftPhone.M.ChatBotInt.Kafka;
using ChatBotInt.Repositories;
using Microsoft.AspNetCore.Http;

namespace SoftPhone.M.ChatBotInt.Controllers.Tests
{
    public class ChatMessageControllerTests
    {
        #region Fields

        ChatMessageController _controller;
        private readonly Mock<IChatMessageRepository> _repository;
        private Mock<IChatService> _service;
        private Mock<IProducerService> _producer;

        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public ChatMessageControllerTests()
        {
            _service = new Mock<IChatService>();
            _producer = new Mock<IProducerService>();
            _repository = new Mock<IChatMessageRepository>();
        }
        private ChatMessageController CreateSut()
        {
            return new ChatMessageController(_service.Object, _producer.Object, _repository.Object);
        }
        #endregion
        [Fact]
        public async Task SaveMessageToDB_ReturnsOkResultAsync()
        {
            // Arrange
            var message = new UnitTests.Chat.ServiceFake().ListChatMessages().FirstOrDefault();
            _repository.Setup(repo => repo.SaveMessageToDB(message)).ReturnsAsync(It.IsAny<int>);
            var sut = CreateSut();
            // Act
            var result = await sut.SaveMessageToDB(message);
            var okResult = result as OkObjectResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task SaveMessageToDB_AddModelStateInvalid_ReturnBadRequest()
        {
            // Arrange
            var message = new UnitTests.Chat.ServiceFake().ListChatMessages().FirstOrDefault(); ;
            _repository.Setup(repo => repo.SaveMessageToDB(message)).Throws(new Exception());
            var sut = CreateSut();
            sut.ModelState.AddModelError("test", "test");
            // Act
            var result = await sut.SaveMessageToDB(message);
            var okResult = result as StatusCodeResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public async Task SaveMessageToDB_AddNullModel_ReturnBadRequest()
        {
            // Arrange
            ChatMessageDTO message = null;
            _repository.Setup(repo => repo.SaveMessageToDB(message)).Throws(new Exception());
            var sut = CreateSut();
            // Act
            var result = await sut.SaveMessageToDB(message);
            var okResult = result as StatusCodeResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<BadRequestResult>(okResult);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public async void GetAllMessagesBySessionId_ReturnsOkResult()
        {
            // Arrange
            var expected = Guid.Parse("94D2EC66-5DA2-4023-9289-A47E19510C8C");
            _repository.Setup(repo => repo.GetAllMessagesBySessionId(It.IsAny<Guid>()))
                .ReturnsAsync(new UnitTests.Chat.ServiceFake().ListChatMessages().Where(x=>x.SessionId== expected));
            var sut = CreateSut();
            // Act
            var result = await sut.GetAllMessagesBySessionId(expected);
            var okResult = result as ObjectResult;
            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ChatMessageDTO>>(viewResult.Value);
            Assert.Equal(expected, model.Select(x=>x.SessionId).FirstOrDefault());
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            //mock.Setup(repo => repo.GetAllMessagesBySessionId(It.IsAny<Guid>())).Returns((Guid i) => Task.FromResult(new UnitTests.Chat.ServiceFake().ListChatMessages().Where(x=>x.SessionId==i)));;
        }

        //[Fact]
        //public async void GetAllMessagesBySessionId_ReturnBadRequestResult()
        //{
        //    //Arrange  
        //    Guid sessionId = Guid.Empty;
        //    //_repository.Setup(repo => repo.GetAllMessagesBySessionId(sessionId))
        //    //   .ReturnsAsync(new UnitTests.Chat.ServiceFake().ListChatMessages());
        //    var sut = CreateSut();
        //    //Act  
        //    var result = await sut.GetAllMessagesBySessionId(sessionId);

        //    // Assert
        //    var viewResult = Assert.IsType<BadRequestResult>(result);
            
        //    var model = Assert.IsAssignableFrom<IEnumerable<ChatMessageDTO>>(viewResult);
        //    Assert.Equal(sessionId, model.Select(x => x.SessionId).FirstOrDefault());
        //    Assert.NotNull(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, viewResult.StatusCode);
        //    //Assert  
        //    //Assert.IsType<BadRequestResult>(data);
        //}


        //#region Util

        //#endregion


    }
}