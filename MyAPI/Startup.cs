﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Extension;

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
            services.AddSingleton(Configuration);
            //services.AddCap(x =>
            //{
            //    // If you are using EF, you need to add the following configuration：
            //    // Notice: You don't need to config x.UseSqlServer(""") again! CAP can autodiscovery.

            //    // If you are using ado.net,you need to add the configuration：
            //    x.UseSqlServer(config =>
            //    {
            //        config.ConnectionString =
            //        Configuration.GetConnectionString("LoggerDatabase");
            //    });

            //    // If you are using Kafka, you need to add the configuration：
            //    x.UseKafka("127.0.0.1:9092");
            //    x.FailedRetryCount = 2;
            //    x.FailedRetryInterval = 5;


            //    // Register Dashboard
            //    x.UseDashboard();

            //    // Register to Consul
            //    x.UseDiscovery(d =>
            //    {
            //        d.DiscoveryServerHostName = "localhost";
            //        d.DiscoveryServerPort = 8500;
            //        d.CurrentNodeHostName = "localhost";
            //        d.CurrentNodePort = 5200;
            //        d.NodeId = 1;
            //        d.NodeName = "CAP No.1 Node";
            //    });
            //});
            //var serviceProvider = services.BuildServiceProvider();
            //var coreprofilerRep = serviceProvider.GetService<ICoreProfilerRepository>();
            //var logger = serviceProvider.GetService<ILogger<CoreProfilerResultFilter>>();
            services.AddMvc(config =>
            {
                //config.Filters.Add(new CoreProfilerResultFilterAttribute());// an instance
                //config.Filters.Add(typeof(CoreProfilerResultFilterAttribute)); // by type
                //config.Filters.Add(new CoreProfilerResultFilter(logger, coreprofilerRep));// an instance
            });
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "192.168.56.101:6379";
                options.InstanceName = "WebInstance";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime, IDistributedCache cache)
        {
            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    var currentTimeUTC = DateTime.UtcNow.ToString();
            //    byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            //    var options = new DistributedCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromSeconds(20));
            //    cache.Set(CacheConst.TimeUTC, encodedCurrentTimeUTC, options);
            //});
            //app.UseApiKeyValidation();
            //app.UseCoreProfiler(true).UseCap();
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
