using Microsoft.AspNetCore.Http;
using MyAPI.Repositorys;
using System.Threading.Tasks;

namespace MyAPI.Middlewares
{
    public class ApiKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;//we have to delegate
        private ICustomersRepository _customersRepository { get; set; }//DI first

        public ApiKeyValidatorsMiddleware(RequestDelegate next, ICustomersRepository repo)
        {
            _next = next;
            _customersRepository = repo;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("user-key"))
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("User Key is missing");
                return;
            }
            else
            {
                if (!_customersRepository.CheckValidUserKey(context.Request.Headers["user-key"]))
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }


}
