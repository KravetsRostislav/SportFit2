using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInt.Repositories.Interfaces
{
    public interface IStatusCodeRepository
    {
        Task<bool> GetStatusCode();
    }
}
