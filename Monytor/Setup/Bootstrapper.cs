using Autofac;
using Monytor.RavenDb;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Raven.Client;
using Raven.Client.Document;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Monytor.Infrastructure;

namespace Monytor.Setup {
    internal class Bootstrapper {
        public async static Task<IContainer> Setup() {
            Logger.Info("Load config");
            var config = LoadConfig();
            Logger.Info("Load database");
            var documentStore = SetupDatabase(config);

            Logger.Info("Setup DI");
            var builder = new ContainerBuilder();
            builder.RegisterInstance(config)
                   .As<IConfigurationRoot>();
            builder.RegisterInstance(documentStore)
                   .As<IDocumentStore>();

            var scheduler = await new StdSchedulerFactory().GetScheduler();

            builder.RegisterInstance(scheduler).As<IScheduler>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x));

            return builder.Build();
        }

        private static DocumentStore SetupDatabase(IConfigurationRoot config) {
            var url = config["database:url"];
            var databaseName = config["database:name"];

            var db = RavenHelper.CreateStore(url, databaseName);

            new SerieIndex().SideBySideExecuteAsync(db);

            return db;
        }

        private static IConfigurationRoot LoadConfig() {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }
    }
}
