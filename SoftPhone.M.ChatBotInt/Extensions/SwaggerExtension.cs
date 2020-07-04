using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SoftPhone.M.ChatBotInt.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("ChatBotInt", new OpenApiInfo
                {
                    Version = "v" + Assembly.GetEntryAssembly().GetName().Version,
                    Title = "Chat Bot Integration Web API",
                    Description = "ASP.NET Core Web API"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                s.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("../swagger/ChatBotInt/swagger.json", $"Chat Bot Integration Web API v{Assembly.GetEntryAssembly().GetName().Version}");
                s.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
