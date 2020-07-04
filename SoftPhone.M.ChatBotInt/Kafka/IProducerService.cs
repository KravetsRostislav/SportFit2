using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Kafka
{
    public interface IProducerService
    {
        Task<DeliveryResult<Null, string>> Produce(Message<Null, string> message);
    }
}
