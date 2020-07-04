using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using SoftPhone.M.ChatBotInt.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Hubs
{
    public class MessagerHub : Hub<IMessagerHub>
    {
        private IProducerService _producer;
        public MessagerHub(IProducerService producerService)
        {
            _producer = producerService;
        }

        public async Task ProduceMessage(string message)
        {
            await _producer.Produce(new Message<Null, string> { Value = message });
        }

        public async Task SendMessageToClients(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}
