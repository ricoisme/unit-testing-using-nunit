using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Infarstructure;

namespace MyAPI.Extension
{
    public static class LoggerExtension
    {
        public static IServiceCollection SetupNlogger(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<INlogger>(new NloggerWrapper(configuration));
        }
    }

}
