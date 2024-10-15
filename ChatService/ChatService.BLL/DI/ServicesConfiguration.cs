using ChatService.BLL.Services;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChatService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddBusinessLogicDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<IChatService, ChatService.BLL.Services.ChatService>()
                    .AddScoped<IMessageService, MessageService>();
        }
    }
}
