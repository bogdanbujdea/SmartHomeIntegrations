using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartHomeIntegrations.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {
                        builder.AddJsonFile("appsettings.json");
                        builder.AddEnvironmentVariables();
                    });
                    webBuilder.UseKestrel(opts =>
                    {
                        opts.ListenAnyIP(5000);
                        opts.ListenAnyIP(5001, options => options.UseHttps());
                    });
                });
    }
}
