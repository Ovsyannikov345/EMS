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

namespace CatalogueService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessDependencies(configuration);
            services.AddMapper();
            services.AddGrpcClients(configuration);
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
            services.AddScoped<IEstateService, EstateService>();
        }

        private static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();
        }
    }
}
