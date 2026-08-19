using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Contracts.Interfaces.Users;
using Task_Persistence.Context;
using Task_Persistence.Repository;
using Task_Persistence.Seed;
using Task_Persistence.Settings;

namespace Task_Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<TaskDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TaskConnectionString"));
            });

            services.Configure<InitialUsersSettings>(
                configuration.GetSection(InitialUsersSettings.SectionName));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<DatabaseSeeder>();

            return services;
        }
    }
}
