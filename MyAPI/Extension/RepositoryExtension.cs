using Microsoft.Extensions.DependencyInjection;
using MyAPI.Repositorys;

namespace MyAPI.Extension
{
    public static class RepositoryExtension
    {
        public static IServiceCollection SetupRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICustomersRepository, CustomersRepository>();
            services.AddSingleton<IEventLogRepository, EventLogRepository>();
            return services;
        }
    }
}
