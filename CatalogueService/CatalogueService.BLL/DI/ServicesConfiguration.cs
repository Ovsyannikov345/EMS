using AutoMapper;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.ClientFactory;
using CatalogueService.BLL.Grpc.Services.IServices;
using System.Reflection;

namespace CatalogueService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddBusinessLogicDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessDependencies(configuration);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<IEstateService, EstateService>();

            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();
        }
    }
}
