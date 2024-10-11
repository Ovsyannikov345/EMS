using CatalogueService.BLL.DI;
using CatalogueService.DI;
using CatalogueService.Middleware;
using CatalogueService.Utilities.Mapping;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CatalogueService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            var configuration = builder.Configuration;

            services.AddBusinessLogicDependencies(configuration);
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Run();
        }
    }
}
