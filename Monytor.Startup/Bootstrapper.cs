using Autofac;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Monytor.Core.Configurations;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Monytor.Startup {
    public class Bootstrapper {
        public async static Task<IContainer> Setup() {
            var builder = new ContainerBuilder();

            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            var logger = loggerFactory.CreateLogger<Bootstrapper>();

            logger.LogInformation("Load config");
            var appConfig = LoadConfig();

            SetupDatabase(builder, logger, appConfig);

            logger.LogInformation("Setup DI");
            await SetupDi(builder, appConfig, loggerFactory);

            return builder.Build();
        }

        private static void SetupDatabase(ContainerBuilder builder, ILogger<Bootstrapper> logger, IConfigurationRoot appConfig) {
            logger.LogInformation("Load database");
            var storageProvider = appConfig.GetValue<StorageProvider>("storageProvider");
            switch (storageProvider) {
                case StorageProvider.PostgreSQL:
                    PostgreSQL.Bootstrapper.SetupDatabaseAndRegisterRepositories(builder, appConfig["storageProviderConnectionString"]);
                    break;
                case StorageProvider.RavenDb:
                    RavenDb.Bootstrapper.SetupDatabaseAndRegisterRepositories(builder, appConfig["database:url"], appConfig["database:name"]);
                    break;
                default: 
                    logger.LogError("The configured value of the setting '{}' is not supported.");
                    throw new NotSupportedException("The configured value of the setting '{}' is not supported.");
            }
            
        }

        private static async Task<ContainerBuilder> SetupDi(ContainerBuilder builder, IConfigurationRoot appConfig, ILoggerFactory loggerFactory) {
            builder.RegisterInstance(appConfig)
                   .As<IConfigurationRoot>();                       
            builder.RegisterInstance(loggerFactory)
                    .As<ILoggerFactory>();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .InstancePerDependency();

            builder.RegisterType<CollectorConfig>();
            builder.RegisterType<SchedulerStartup>();
            builder.RegisterType<AutofacJobFactory>().SingleInstance();

            var config = new CollectorConfigCreator(appConfig["collectorConfigFileName"]);
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

        private static IConfigurationRoot LoadConfig() {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .Build();
        }
    }
}