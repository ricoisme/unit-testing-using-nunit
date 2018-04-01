using Microsoft.AspNetCore.Builder;
using MyAPI.Middlewares;

namespace MyAPI.Extension
{
    public static class ApiKeyValidatorsExtension
    {
        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyValidatorsMiddleware>();
            return app;
        }
    }
}
