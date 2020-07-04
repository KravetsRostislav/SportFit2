
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using NLog;
using Microsoft.Extensions.Configuration;
using ChatBotInt.Repositories.Interfaces;

using Services.Status;

namespace SoftPhone.M.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusCodeController : ControllerBase
    {
        private readonly IStatusCodeRepository _statusCodeRepository;
        private readonly IStatusCodeService _statusCodeService;
        private readonly IConfiguration iConfig;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public StatusCodeController(
            IStatusCodeService statusCodeService,
            IStatusCodeRepository statusCodeRepository,
            IConfiguration iConfig
            )
        {
            _statusCodeService = statusCodeService;
            _statusCodeRepository = statusCodeRepository;
            this.iConfig = iConfig;
        }


        [HttpGet("GetStatusCode")]
        public async Task<IActionResult> GetStatusCode()
        {
            try
            {
                var statusCodeAnswer = await _statusCodeService.GetStatusCode();
                _logger.Debug($"{"StatusCodeController:",-20} >>> {"GetStatusCode",-20} >>> {"StatusCode answer:",-10} {statusCodeAnswer}.");
                if (statusCodeAnswer != true)
                    return BadRequest(statusCodeAnswer.ToString());
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{"Message:",-20}{e.Message,-20} >>> StackTrace: {e.StackTrace,20}.");
                return BadRequest(e);
            }
        }
    }
}