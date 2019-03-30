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
using Monytor.Core.Services;
using Monytor.Domain.Services;
using Monytor.Implementation;

namespace Monytor.Startup {
    public class Bootstrapper {
        public async static Task<IContainer> Setup(IConfiguration configuration) {
            var builder = new ContainerBuilder();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            var logger = loggerFactory.CreateLogger<Bootstrapper>();

            var schedulerConfiguration = configuration.GetSection("SchedulerConfiguration")
                .Get<SchedulerConfiguration>();
            builder.RegisterInstance(schedulerConfiguration);

            SetupDatabase(builder, logger, schedulerConfiguration);

            logger.LogInformation("Setup DI");
            await SetupDi(builder, configuration, loggerFactory);

            return builder.Build();
        }

        private static void SetupDatabase(ContainerBuilder builder, ILogger<Bootstrapper> logger, SchedulerConfiguration schedulerConfiguration) {
            logger.LogInformation("Load database");
            switch (schedulerConfiguration.StorageProvider) {
                case StorageProvider.PostgreSql:
                    PostgreSQL.Bootstrapper.SetupDatabaseAndRegisterRepositories(builder, schedulerConfiguration.StorageProviderConnectionString);
                    break;
                case StorageProvider.RavenDb:
                    RavenDb.Bootstrapper.SetupDatabaseAndRegisterRepositories(builder, schedulerConfiguration.StorageProviderConnectionString);
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

            
            builder.RegisterType<SchedulerCollectorConfigService>().SingleInstance().As<ISchedulerCollectorConfigService>();
            builder.RegisterType<SchedulerStartup>();
            builder.RegisterType<AutofacJobFactory>().SingleInstance();
            
            SetupCollectors(builder);
            SetupVerifiers(builder);
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

                if(behavior ==null)
                    continue;
                builder.RegisterType(behavior).Keyed(notification.FullName, typeof(NotificationBehaviorBase));
            }
        }

        private static void SetupVerifiers(ContainerBuilder builder) {
            var verifiers = ImplementationTypeLoader.LoadAllConcreteTypesOf(typeof(Verifier));
            foreach (var verifier in verifiers) {
                var behavior = ImplementationTypeLoader.LoadBehavior(typeof(VerifierBehavior<>), verifier);
                if(behavior == null)
                    continue;
                builder.RegisterType(behavior).Keyed(verifier.FullName, typeof(VerifierBehaviorBase));
            }
        }

        private static void SetupCollectors(ContainerBuilder builder) {
            var collectors = ImplementationTypeLoader.LoadAllConcreteTypesOf(typeof(Collector));
            foreach (var collector in collectors) {
                var behavior = ImplementationTypeLoader.LoadBehavior(typeof(CollectorBehavior<>), collector);
                if(behavior == null)
                    continue;
                builder.RegisterType(behavior).Keyed(collector.FullName, typeof(CollectorBehaviorBase));
            }
        }       
    }
}