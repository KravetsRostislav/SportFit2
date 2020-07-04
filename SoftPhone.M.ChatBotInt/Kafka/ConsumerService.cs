using ChatBotInt.Repositories.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using SoftPhone.M.ChatBotInt.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Kafka
{
    public class ConsumerService : IHostedService, IDisposable
    {
        private Thread _pollLoopThread;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ConsumerConfig _consumerConfig = new ConsumerConfig();
        private string _topic;
        private IHubContext<MessagerHub, IMessagerHub> _messagerHubContext;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public ConsumerService(IConfiguration config, IHubContext<MessagerHub, IMessagerHub> messagerHubContext)
        {
            config.GetSection("Consumer").Bind(_consumerConfig);

            if (_consumerConfig.EnablePartitionEof != null)
            {
                throw new Exception("shouldn't allow this to be set in config.");
            }

            _consumerConfig.EnableAutoCommit = false;

            _topic = config.GetValue<string>("Topic");
            _messagerHubContext = messagerHubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _pollLoopThread = new Thread(() => {
                try
                {
                    using (var consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build())
                    //using (var consumer = new ConsumerBuilder<Null, SendMessageModel>(_consumerConfig).Build())
                    {
                        consumer.Subscribe(_topic);

                        try
                        {
                            while (!_cancellationTokenSource.IsCancellationRequested)
                            {
                                var cr = consumer.Consume(_cancellationTokenSource.Token);

                                _logger.Info($"ConsumerService. Message From Kafka: {cr.Message.Value}");
                                _messagerHubContext.Clients.All.SendMessage($"received: {cr.Message.Value}");
                            }
                        }
                        catch (OperationCanceledException) { }

                        consumer.Close();
                    }
                }
                catch
                {
                    // todo
                }
            });

            _pollLoopThread.Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _cancellationTokenSource.Cancel();
                _pollLoopThread.Join();
            });
        }

        public void Dispose() { }
    }
}
