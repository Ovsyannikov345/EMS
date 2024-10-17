using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ChatService.DI
{
    public static class ServicesConfiguration
    {
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
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
                                          .AllowAnyMethod()
                                          .AllowCredentials());
            });
        }
    }
}
