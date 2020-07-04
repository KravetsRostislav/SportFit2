using ChatBotInt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Status
{
    public class StatusCodeService : IStatusCodeService
    {

        private readonly IStatusCodeRepository _statusCodeRepository;

        public StatusCodeService(IStatusCodeRepository statusCodeRepository)
        {
            _statusCodeRepository = statusCodeRepository;
        }

        public async Task<bool> GetStatusCode()
        {
            return await _statusCodeRepository.GetStatusCode();
        }
    }
}
