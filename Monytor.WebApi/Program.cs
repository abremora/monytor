using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Monytor.WebApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, configuration) =>
                    {
                        configuration.AddCommandLine(args)
                            .SetBasePath(GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile("settings/appsettings.docker.json", true);
                    });
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetCurrentDirectory() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}
