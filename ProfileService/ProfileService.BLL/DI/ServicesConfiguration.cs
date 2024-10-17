using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Services;
using ProfileService.BLL.Utilities.Mapping;
using System.Reflection;

namespace ProfileService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
