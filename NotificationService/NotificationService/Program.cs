using MassTransit;
using NotificationService.BLL.DI;
using NotificationService.DAL.DI;
using NotificationService.DI;
using NotificationService.Middleware;
using NotificationService.Utilities.Mapping;
using Serilog;
using System.Reflection;

namespace NotificationService
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

            services.AddDataAccessDependencies(configuration);
            services.AddBusinessLogicDependencies();
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));
            services.AddGrpc(_ => _.Interceptors.Add<GrpcExceptionHandlingInterceptor>());
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"]);

                    cfg.ConfigureEndpoints(context);
                });
            });

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
