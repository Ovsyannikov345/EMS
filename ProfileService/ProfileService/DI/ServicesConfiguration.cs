using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProfileService.BLL.Utilities.Mapping;
using ProfileService.DAL.Repositories.IRepositories;
using ProfileService.DAL.Repositories;
using System.Security.Claims;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Services;
using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.DataContext;

namespace ProfileService.DI
{
    public static class ServicesConfiguration
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<ProfileDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ProfileDatabase");

            options.UseNpgsql(connectionString);
        });

        public static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration["Auth0:Domain"]}/";
                options.Audience = configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                };
            });
        }

        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            string[]? corsOrigins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? throw new InvalidOperationException("Cors origins are not defined");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder.WithOrigins(corsOrigins)
                                          .AllowAnyHeader()
                                          .AllowCredentials());
            });
        }
        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        public static void AddDataAccessRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProfileRepository, ProfileRepository>();
        }

        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
