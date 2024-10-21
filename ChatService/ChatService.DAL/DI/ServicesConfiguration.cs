using ChatService.DAL.Data;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Grpc.Services;
using ChatService.DAL.Grpc.Services.Profile;
using ChatService.DAL.Grpc.Services.Estate;
using ChatService.DAL.Repositories;
using ChatService.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.DAL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

            services.AddScoped<IChatRepository, ChatRepository>()
                    .AddScoped<IMessageRepository, MessageRepository>();

            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddGrpcClient<EstateGrpcServiceProto.EstateGrpcServiceProtoClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("CatalogueService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>()
                    .AddScoped<IEstateGrpcClient, EstateGrpcClient>();
        }
    }
}
