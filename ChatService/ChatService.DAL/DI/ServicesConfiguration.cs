using ChatService.DAL.Data;
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

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IChatRepository, ChatRepository>()
                    .AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}
