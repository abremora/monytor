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
using Monytor.Core.Repositories;
using Monytor.Core.Configurations;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Monytor.Startup {
    public class Bootstrapper {
        public async static Task<IContainer> Setup() {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            var logger = loggerFactory.CreateLogger<Bootstrapper>();

            logger.LogInformation("Load config");
            var appConfig = LoadConfig();
            logger.LogInformation("Load database");
            var documentStore = SetupDatabase(appConfig);
            logger.LogInformation("Setup DI");
            var builder = await SetupDi(appConfig, documentStore, loggerFactory);
            
            return builder.Build();
        }

        private static async Task<ContainerBuilder> SetupDi(IConfigurationRoot appConfig, DocumentStore documentStore, ILoggerFactory loggerFactory) {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(appConfig)
                   .As<IConfigurationRoot>();
            builder.RegisterInstance(documentStore)
                   .As<IDocumentStore>();
            builder.RegisterType<SeriesRepository>()
                    .As<ISeriesRepository>();
            builder.RegisterInstance(loggerFactory)
                    .As<ILoggerFactory>();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .InstancePerDependency();

            builder.RegisterType<CollectorConfig>();
            builder.RegisterType<SchedulerStartup>();
            builder.RegisterType<AutofacJobFactory>().SingleInstance();

            var config = new CollectorConfigCreator();
            var collectorConfig = config.LoadConfig();

            builder.RegisterInstance(collectorConfig);

            SetupCollectors(builder, collectorConfig);
            SetupVerfiers(builder, collectorConfig);
            SetupNotifications(builder, collectorConfig);

            var scheduler = await new StdSchedulerFactory().GetScheduler();

            builder.RegisterInstance(scheduler).As<IScheduler>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x));
            return builder;
        }

        private static void SetupNotifications(ContainerBuilder builder, CollectorConfig collectorConfig) {
            foreach (var notification in collectorConfig.Notifications) {
                var b = ConfigCreator.LoadBehavior(typeof(NotificationBehavior<>), notification.GetType());
                builder.RegisterType(b).Named(notification.Id, typeof(NotificationBehaviorBase));
                builder.RegisterInstance(notification).Named(notification.Id, typeof(Notification));
            }
        }

        private static void SetupVerfiers(ContainerBuilder builder, CollectorConfig collectorConfig) {
            var distinctVerifiers = collectorConfig.Collectors
                .SelectMany(x => x.Verifiers.Select(y => y.GetType())).Distinct();

            foreach (var verifier in distinctVerifiers) {

                var b = ConfigCreator.LoadBehavior(typeof(VerfiyBehavior<>), verifier);
                builder.RegisterType(b).Keyed(verifier, typeof(VerifierBehaviorBase));
            }
        }

        private static void SetupCollectors(ContainerBuilder builder, CollectorConfig collectorConfig) {
            var distinctCollectors = collectorConfig.Collectors.Select(x => x.GetType()).Distinct();
            foreach (var collector in distinctCollectors) {
                var b = ConfigCreator.LoadBehavior(typeof(CollectorBehavior<>), collector);
                builder.RegisterType(b).Keyed(collector, typeof(CollectorBehaviorBase));
            }
        }

        private static DocumentStore SetupDatabase(IConfigurationRoot config) {
            var url = config["database:url"];
            var databaseName = config["database:name"];

            var db = RavenHelper.CreateStore(url, databaseName);

            new SeriesIndex().SideBySideExecute(db);
            new SeriesByDayIndex().SideBySideExecute(db);
            new SeriesByHourIndex().SideBySideExecute(db);
            new TagGroupMapReduceIndex().SideBySideExecute(db);
            return db;
        }

        private static IConfigurationRoot LoadConfig() {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}