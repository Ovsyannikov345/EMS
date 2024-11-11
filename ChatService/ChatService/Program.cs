using ChatService.BLL.DI;
using ChatService.DAL.DI;
using ChatService.DI;
using ChatService.Hubs;
using ChatService.Middleware;
using ChatService.Utilities.Mapping;
using Serilog;
using System.Reflection;

namespace ChatService
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
            services.AddBusinessLogicDependencies(configuration);
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));
            services.AddSignalR();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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

            app.UseWebSockets();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}
