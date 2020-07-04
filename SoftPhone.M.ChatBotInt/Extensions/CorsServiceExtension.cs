using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Extensions
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsSettings(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });

            return services;
        }

        public static IApplicationBuilder UseCorsSettings(this IApplicationBuilder app)
        {
            app.UseCors("AllowAllOrigin");
            return app;
        }
    }
}
