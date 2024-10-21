using Microsoft.Extensions.DependencyInjection;
using NotificationService.BLL.Services.IServices;
using NotificationService.BLL.Utilities.Mapping;
using System.Reflection;

namespace NotificationService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddBusinessLogicDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<INotificationService, Services.NotificationService>();
        }
    }
}
