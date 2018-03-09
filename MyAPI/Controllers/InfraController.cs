using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Infra")]
    public class InfraController : Controller
    {
        [HttpGet]
        public string Get() => System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        [HttpGet("GetCustomAttribute/{attributeName?}", Name = "Infra_GetCustomAttribute")]
        public string[] GetCustomAttribute(string attributeName = "")
        {
            MyProperty myProperty = new MyProperty();
            foreach (var itemAttribute in typeof(Startup).GetTypeInfo().Assembly.GetCustomAttributes())
            {
                if (itemAttribute is AssemblyInformationalVersionAttribute)
                {
                    myProperty.InformationalVersion = (itemAttribute as AssemblyInformationalVersionAttribute)
                        .InformationalVersion;
                }
                else if (itemAttribute is AssemblyCopyrightAttribute)
                {
                    myProperty.Copyright = (itemAttribute as AssemblyCopyrightAttribute).Copyright;
                }
                else if (itemAttribute is AssemblyProductAttribute)
                {
                    myProperty.Product = (itemAttribute as AssemblyProductAttribute).Product;
                }
                else if (itemAttribute is AssemblyCompanyAttribute)
                {
                    myProperty.Company = (itemAttribute as AssemblyCompanyAttribute).Company;
                }
                else if (itemAttribute is TargetFrameworkAttribute)
                {
                    myProperty.TargetFramework = (itemAttribute as TargetFrameworkAttribute).FrameworkName;
                }
            }
            return typeof(MyProperty).GetProperties()
                .Select(e => e.GetValue(myProperty)?.ToString())
                .ToArray();
        }

        [HttpGet("GetRequestInformation/{headerKey?}", Name = "Infra_GetRequestInformation")]
        public string[] GetRequestInformation(string headerKey = "")
        {
            var contextServerIp = this.Request.HttpContext.Connection.LocalIpAddress;
            var contextClientIp = this.Request.HttpContext.Connection.RemoteIpAddress;
            var headerClientIp = this.Request.Headers[string.IsNullOrEmpty(headerKey) ? "X-Forwarded-For" : headerKey];
            var otherHeaders = this.Request.Headers
                .Select(kv => $"key:{kv.Key},value:{kv.Value}");
            return otherHeaders
                .Append($"ServerIpFromHttpcontext:{contextServerIp.ToString()}")
                .Append($"ClientIpFromHttpcontext:{contextClientIp.ToString()}")
                .Append($"ClientIpFromHeaders:{headerClientIp.ToString()}")
                .ToArray();
        }
    }

    public class MyProperty
    {
        public string InformationalVersion { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
        public string TargetFramework { get; set; }
        public string Copyright { get; set; }
    }
}