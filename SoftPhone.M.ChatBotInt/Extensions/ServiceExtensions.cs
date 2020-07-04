
using Microsoft.Extensions.DependencyInjection;
using Services.Chat;
using Services.Status;
using SoftPhone.M.ChatBotInt.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IStatusCodeService, StatusCodeService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddHostedService<ConsumerService>();

            services.AddSignalR();

            return services;
        }
    }
}
