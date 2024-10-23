using AutoMapper;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.BLL.Producers.IProducers;
using CatalogueService.BLL.Producers;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit;

namespace CatalogueService.BLL.DI
{
    public static class ServicesConfiguration
    {
        public static void AddBusinessLogicDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessDependencies(configuration);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddMassTransit(configuration);

            services.AddScoped<IEstateService, EstateService>()
                    .AddScoped<IEstateFilterService, EstateFilterService>();
        }

        public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"]);
                });
            });

            services.AddTransient<INotificationProducer, NotificationProducer>();
        }
    }
}
