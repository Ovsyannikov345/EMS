using ProfileService.BLL.DI;
using ProfileService.BLL.Grpc.Services;
using ProfileService.Utilities.Mapping;
using ProfileService.DI;
using ProfileService.Middleware;
using System.Reflection;
using Serilog;
using ProfileService.Extensions;

namespace ProfileService
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

            services.AddApplicationDependencies(configuration);

            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAutoValidation();
            services.AddGrpc(_ => _.Interceptors.Add<GrpcExceptionHandlingInterceptor>());
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
            app.MapGrpcService<ProfileGrpcService>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
