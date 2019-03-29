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
using Monytor.Implementation;

namespace Monytor.Startup {
    public class Bootstrapper {
        public async static Task<IContainer> Setup(IConfiguration configuration) {
            var builder = new ContainerBuilder();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            var logger = loggerFactory.CreateLogger<Bootstrapper>();
            SetupDatabase(builder, logger, configuration);

            logger.LogInformation("Setup DI");
            await SetupDi(builder, configuration, loggerFactory);

            return builder.Build();
        }

        private static void SetupDatabase(ContainerBuilder builder, ILogger<Bootstrapper> logger, IConfiguration appConfig) {
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
                    logger.LogError("The configured value of the setting 'storageProvider' is not supported.");
                    throw new NotSupportedException("The configured value of the setting 'storageProvider' is not supported.");
            }
            
        }

        private static async Task<ContainerBuilder> SetupDi(ContainerBuilder builder, IConfiguration appConfig, ILoggerFactory loggerFactory) {
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

            SetupCollectors(builder);
            SetupVerfiers(builder);
            SetupNotifications(builder);

            var scheduler = await new StdSchedulerFactory().GetScheduler();

            builder.RegisterInstance(scheduler).As<IScheduler>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x));
            return builder;
        }

        private static void SetupNotifications(ContainerBuilder builder) {
            var notifications = ImplementationTypeLoader.LoadAllConcreteTypesOf(typeof(Notification));
            foreach (var notification in notifications) {
                var behavior = ImplementationTypeLoader.LoadBehavior(typeof(NotificationBehavior<>), notification);
                builder.RegisterType(behavior).Keyed(notification.FullName, typeof(NotificationBehaviorBase));
            }
        }

        private static void SetupVerfiers(ContainerBuilder builder) {
            var verifiers = ImplementationTypeLoader.LoadAllConcreteTypesOf(typeof(Verifier));
            foreach (var verifier in verifiers) {
                var behavior = ImplementationTypeLoader.LoadBehavior(typeof(VerfiyBehavior<>), verifier);
                builder.RegisterType(behavior).Keyed(verifier.FullName, typeof(VerifierBehaviorBase));
            }
        }

        private static void SetupCollectors(ContainerBuilder builder) {
            var collectors = ImplementationTypeLoader.LoadAllConcreteTypesOf(typeof(Collector));
            foreach (var collector in collectors) {
                var behavior = ImplementationTypeLoader.LoadBehavior(typeof(CollectorBehavior<>), collector);
                builder.RegisterType(behavior).Keyed(collector.FullName, typeof(CollectorBehaviorBase));
            }
        }       
    }
}