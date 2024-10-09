﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ProfileService.DI
{
    public static class ServicesConfiguration
    {
        public static void AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthenticationBearer(configuration);
            services.AddCorsPolicy(configuration);
            services.AddGrpc();
        }

        private static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
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

        private static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
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
    }
}