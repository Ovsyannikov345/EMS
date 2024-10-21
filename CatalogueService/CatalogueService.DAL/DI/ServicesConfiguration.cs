using CatalogueService.DAL.Data;
using CatalogueService.DAL.Grpc.Services.IServices;
using CatalogueService.DAL.Grpc.Services;
using CatalogueService.DAL.Repositories;
using CatalogueService.DAL.Repositories.IRepositories;
using CatalogueService.DAL.Utilities.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CatalogueService.DAL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EstateDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

            services.AddScoped<IEstateRepository, EstateRepository>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();
        }
    }
}
