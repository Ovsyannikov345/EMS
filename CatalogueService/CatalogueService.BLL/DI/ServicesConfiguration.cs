using AutoMapper;
using CatalogueService.BLL.Services;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Mapping;
using CatalogueService.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
