using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Services;
using Task_Application.Contracts.Interfaces.Users;
using Task_Infrastructure.Services;
using Task_Infrastructure.Services.Security;
using Task_Infrastructure.Settings;

namespace Task_Infrastructure
{
    public static class InfrastructureServicesRegistration
    {

        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            // این ابزار خود دات‌نت برای دسترسی به HttpContext است
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
