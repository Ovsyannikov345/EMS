using ChatService.BLL.Producers;
using ChatService.BLL.Producers.IProducers;
using ChatService.BLL.Services;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Mapping;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChatService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddBusinessLogicDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddMassTransit(configuration);

            services.AddScoped<IChatService, ChatService.BLL.Services.ChatService>()
                    .AddScoped<IMessageService, MessageService>();
        }

        public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"]);
                });
            });

            services.AddTransient<INotificationProducer, NotificationProducer>();
        }
    }
}
