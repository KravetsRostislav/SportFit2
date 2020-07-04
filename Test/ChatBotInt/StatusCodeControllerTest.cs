using Xunit;
using Moq;
using ChatBotInt.Repositories.Interfaces;
using NLog;
using Microsoft.AspNetCore.Mvc;
using SoftPhone.M.Core.Controllers;
using Services.Status;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace SoftPhone.M.ChatBotInt.Controllers.Tests
{
    public class StatusCodeControllerTest
    {
        #region Fields
        private readonly Mock<IStatusCodeRepository> _statusCodeRepository;
        private readonly Mock<IStatusCodeService> _statusCodeService;
        private readonly Mock<IConfiguration> _iConfig;

        Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctor

        public StatusCodeControllerTest()
        {
            _statusCodeRepository = new Mock<IStatusCodeRepository>();
            _statusCodeService = new Mock<IStatusCodeService>();
            _iConfig = new Mock<IConfiguration>();

        }
        #endregion
        [Fact]
        public async void GetStatusCode_ReturnOkResult()
        {
            // Arrange
            _statusCodeService.Setup(repo => repo.GetStatusCode()).ReturnsAsync(true);
            var sut = CreateSut();
            // Act
            var result = await sut.GetStatusCode();
            var okResult = result as StatusCodeResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async void GetStatusCode_ReturnBadRequest()
        {
            // Arrange
            var sut = CreateSut();
            _statusCodeService.Setup(repo => repo.GetStatusCode()).ReturnsAsync(false);
            // Act
            var result = await sut.GetStatusCode();
            var objectResult = result as BadRequestObjectResult;
            // Assert
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(objectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        #region Util

        private StatusCodeController CreateSut()
        {
            return new StatusCodeController(_statusCodeService.Object, _statusCodeRepository.Object, _iConfig.Object);
        }

        #endregion


    }
}