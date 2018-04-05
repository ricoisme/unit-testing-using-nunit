using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace MyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("hosting.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("coreprofiler.json", optional: false, reloadOnChange: true)
                .Build();
            BuildWebHost(args, config).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)//override from hosting.json file
                                                //.ConfigureLogging((hostingContext, logging) =>
                                                //{
                                                //    //options = serviceProvider.GetRequiredService<IOptionsMonitor<AzureFileLoggerOptions>>();
                                                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                                //    logging.AddConsole();
                                                //    logging.AddDebug();
                                                //})
                .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                .UseNLog()  // NLog: setup NLog for Dependency injection
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
                    options.Limits.MaxConcurrentConnections = 1000;
                    options.Limits.MaxConcurrentUpgradedConnections = 1000;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                })
                .Build();
    }
}
