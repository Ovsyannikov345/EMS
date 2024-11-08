using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.DAL.DataContext;
using ProfileService.DAL.Repositories.IRepositories;
using ProfileService.DAL.Repositories;
using StackExchange.Redis;
using ProfileService.DAL.CacheProvider.ICacheProvider;
using ProfileService.DAL.CacheProvider;

namespace ProfileService.DAL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProfileDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ProfileDatabase")));

            services.AddRepositories();

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
            services.AddTransient<IRedisCacheProvider, RedisCacheProvider>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProfileRepository, ProfileRepository>()
                    .AddScoped<IProfileInfoVisibilityRepository, ProfileInfoVisibilityRepository>();
        }
    }
}
