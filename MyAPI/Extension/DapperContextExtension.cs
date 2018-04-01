using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Infarstructure;

namespace MyAPI.Extension
{
    public static class DapperContextExtension
    {
        public static IServiceCollection SetupDapperContext(this IServiceCollection services
            , IConfiguration configuration, string connectionStringKey, string coreProfilerKey = "EnableCoreProfiler")
        {
            services.AddSingleton<IDapperContext>(new DapperContext(configuration, connectionStringKey, coreProfilerKey));
            return services;
        }
    }
}
