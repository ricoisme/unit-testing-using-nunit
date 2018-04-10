using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MyAPI.Filters
{
    public class AddHeaderResultFilterAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;
        public AddHeaderResultFilterAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Add(
                _name, new string[] { _value });
            return base.OnResultExecutionAsync(context, next);
        }
    }
}
