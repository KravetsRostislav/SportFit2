using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Hubs
{
    public interface IMessagerHub
    {
        Task ProduceMessage(string message);

        Task SendMessage(string meassage);
    }
}
