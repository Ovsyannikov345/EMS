using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Services;
using ProfileService.BLL.Utilities.Mapping;
using System.Reflection;
using ProfileService.DAL.DI;
using System.Runtime.Versioning;

namespace ProfileService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessDependencies(configuration);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<IUserProfileService, UserProfileService>()
                    .AddScoped<IProfileInfoVisibilityService, ProfileInfoVisibilityService>()
                    .AddScoped<IProfileImageService, ProfileImageService>();
        }
    }
}
