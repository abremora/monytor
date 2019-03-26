using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Monytor.WebApi {
    public class Program {
        public static void Main(string[] args) {
            var configuration = new ConfigurationBuilder()
               .AddCommandLine(args)
               .SetBasePath(GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.{env.EnvironmentName}.json", true)
               .Build();

            BuildWebHost(args, configuration)
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();

        private static string GetCurrentDirectory() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}
