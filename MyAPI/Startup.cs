using CoreProfiler.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Extension;
using MyAPI.Filters;

namespace MyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.SetupDapperContext(Configuration, "LoggerDatabase");
            services.SetupRepositories();
            //var serviceProvider = services.BuildServiceProvider();
            //var coreprofilerRep = serviceProvider.GetService<ICoreProfilerRepository>();
            //var logger = serviceProvider.GetService<ILogger<CoreProfilerResultFilter>>();
            services.AddMvc(config =>
            {
                config.Filters.Add(new CoreProfilerResultFilterAttribute());// an instance
                //config.Filters.Add(typeof(CoreProfilerResultFilterAttribute)); // by type
                //config.Filters.Add(new CoreProfilerResultFilter(logger, coreprofilerRep));// an instance
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseApiKeyValidation();
            app.UseCoreProfiler(true);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })
            .UseStaticFiles()
            .UseMvc();
            //.UseMvc(routes =>
            //{
            //    routes.MapRoute("api", "api/{controller=Infra}/{action=GetOs}/{id?}");
            //    routes.MapRoute("default", "{controller=values}/{action=Index}/{id?}");
            //});
        }
    }
}
