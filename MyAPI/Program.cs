using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace MyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("hosting.json", optional: false)
                .Build();
            BuildWebHost(args, config).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)//override from hosting.json file
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
