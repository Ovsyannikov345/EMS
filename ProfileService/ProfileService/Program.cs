using ProfileService.DI;
using ProfileService.Middleware;

namespace ProfileService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthenticationBearer(builder.Configuration);
            services.AddCorsPolicy(builder.Configuration);
            services.AddDatabaseContext(builder.Configuration);
            services.AddDataAccessRepositories();
            services.AddBusinessLogicServices();
            services.AddMapper();

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
