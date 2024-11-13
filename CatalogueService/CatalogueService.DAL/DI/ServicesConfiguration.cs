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
using Minio;
using CatalogueService.DAL.BlobStorages.IBlobStorages;
using CatalogueService.DAL.BlobStorages;

namespace CatalogueService.DAL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EstateDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

            services.AddScoped<IEstateRepository, EstateRepository>()
                    .AddScoped<IEstateFilterRepository, EstateFilterRepository>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(configuration.GetConnectionString("ProfileService")!);
            });

            services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(configuration["Minio:Endpoint"])
                .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
                .WithSSL(false)
                .Build());

            services.AddScoped<IMinioStorage, MinioStorage>();
        }
    }
}
