using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Kafka
{
    public class ProducerService : IProducerService, IDisposable
    {
        private IProducer<Null, string> _producer;
        private string _topic;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public ProducerService(IConfiguration config)
        {
            var producerConfig = new ProducerConfig();
            config.GetSection("Producer").Bind(producerConfig);
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            _topic = config.GetValue<string>("Topic");
        }

        public Task<DeliveryResult<Null, string>> Produce(Message<Null, string> message)
        {
            _logger.Info($"ProducerService. Produce Message To Kafka: {message.Value}");
            return _producer.ProduceAsync(_topic, message);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
