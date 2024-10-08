using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Services;
using ProfileService.DAL.DI;
using ProfileService.BLL.Utilities.Mapping;

namespace ProfileService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessDependencies(configuration);
            services.AddMapper();
            services.AddBusinessLogicServices();
        }

        private static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        private static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
