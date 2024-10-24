using CatalogueService.BLL.DI;
using CatalogueService.BLL.Grpc.Services;
using CatalogueService.DI;
using CatalogueService.Middleware;
using CatalogueService.Utilities.Mapping;
using Serilog;
using System.Reflection;

namespace CatalogueService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, loggerConfig) =>
                loggerConfig.WriteTo.Console()
                            .WriteTo.File("log.txt"));

            var services = builder.Services;

            var configuration = builder.Configuration;

            services.AddBusinessLogicDependencies(configuration);
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAutoValidation();
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));
            services.AddGrpc(_ => _.Interceptors.Add<GrpcExceptionHandlingInterceptor>());

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
            app.MapGrpcService<EstateGrpcService>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Run();
        }
    }
}
