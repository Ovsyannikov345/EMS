using NotificationService.BLL.DI;
using NotificationService.DAL.DI;
using NotificationService.DI;
using NotificationService.Middleware;
using NotificationService.Utilities.Mapping;
using System.Reflection;

namespace NotificationService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            var configuration = builder.Configuration;

            services.AddDataAccessDependencies(configuration);
            services.AddBusinessLogicDependencies();
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
