using Kentimce.Application.Abstractions;
using Kentimce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Kentimce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IJWTTokenService, JWTTokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecurityKey"])),
                        // Saat farkı toleransını sıfırla
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
