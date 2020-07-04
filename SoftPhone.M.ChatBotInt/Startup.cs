using ChatBotInt.Repositories;
using ChatBotInt.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoftPhone.M.ChatBotInt.Extensions;
using SoftPhone.M.ChatBotInt.Hubs;
using Microsoft.Extensions.Hosting;

namespace SoftPhone.M.ChatBotInt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocumentation();
            //services.AddCorsSettings();

            string connectionStringCore = Configuration.GetConnectionString("ConnectionStringCore");
            services.AddTransient<IStatusCodeRepository, StatusCodeRepository>(provider => new StatusCodeRepository(connectionStringCore));
            services.AddTransient<IChatMessageRepository, ChatMessageRepository>(provider => new ChatMessageRepository(connectionStringCore));

            string connectionStringChat = Configuration.GetConnectionString("ConnectionStringChat");
            services.AddTransient<IChatMessageRepository, ChatMessageRepository>(provider => new ChatMessageRepository(connectionStringChat));
            

            services.AddServices();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())            
                app.UseDeveloperExceptionPage();            
            else            
                app.UseHsts();
            

            app.UseCorsSettings();
            app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<MessagerHub>("/messager");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessagerHub>("/messager");
            });
        }
    }
}
