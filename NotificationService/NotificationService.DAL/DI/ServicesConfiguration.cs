using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.DAL.Data;
using NotificationService.DAL.Grpc.Services;
using NotificationService.DAL.Grpc.Services.IServices;
using NotificationService.DAL.Grpc.Services.Profile;
using NotificationService.DAL.Repositories;
using NotificationService.DAL.Repositories.IRepositories;

namespace NotificationService.DAL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();
        }
    }
}
