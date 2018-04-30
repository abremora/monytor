using Autofac;
using CommandLine;
using Monytor.Infrastructure;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Monytor.Setup;
using Monytor.Core.Configurations;

namespace Monytor {
    class Program {
        static void Main(string[] args) {
            Logger.Text(CommandLine.Text.HeadingInfo.Default);
            Logger.Text(CommandLine.Text.CopyrightInfo.Default);
            Logger.NewLine();
            Logger.Text("Start application with '--help' for assistance.");
            Logger.NewLine();

            try {
                var config = new CollectorConfigCreator();
                var parser = new Parser(x => x.CaseSensitive = false);
                var options = parser.ParseArguments<ConsoleArguments>(args)
                    .WithParsed(o => { if (o.CreateDefaultConfig) config.CreateDefaultConfig(); })                    ;
                
                if (options.Tag == ParserResultType.NotParsed) {
                    goto End;
                }

                var parsedResult = options as Parsed<ConsoleArguments>;

                if (parsedResult.Value.CreateDefaultConfig) {
                    goto End;
                }

                var container = Bootstrapper.Setup();
                if (!config.HasConfig()) {                    
                    Logger.Warning($"Config file '{config.ConfigFileName}' not found. Create default config.\nUse --help for further assistance.");
                }

                RunAsync(container.Result).GetAwaiter().GetResult();         
            }
            catch (Exception ex) {
                Logger.Error(ex);                
            }

            End:
            Logger.Text("Press <ENTER> to close the application");
            Console.ReadLine();
        }

        private static async Task RunAsync(IContainer container) {
            IScheduler scheduler = null;
            try {
                NameValueCollection props = new NameValueCollection                 {
                    { "quartz.serializer.type", "binary" }
                };

                scheduler = container.Resolve<IScheduler>();
                scheduler.JobFactory = new AutofacJobFactory(container);

                await scheduler.Start();
                await ConfigScheduler(scheduler, container);

                Logger.Info("Scheduler started");
                Logger.Text("Press <ENTER> to close the application");
                Logger.NewLine();
                Console.ReadLine();
            }
            catch (Exception e) {
                Logger.Error(e);
            }
            finally {
                await scheduler.Shutdown();
            }
        }

        private static async Task ConfigScheduler(IScheduler scheduler, IContainer container) {
            var collectorConfig = container.Resolve<CollectorConfig>();
            foreach (var collector in collectorConfig.Collectors) {
                Logger.Info("Register: " + collector.GetType().Name);

                var dic = new Dictionary<string, object> {
                    { "CollectorType", collector }
                };
                IJobDetail job = JobBuilder.Create<GlobalCollectorJob>()
                  .WithIdentity(collector.GetType().Name, "CollectorGroup")
                  .SetJobData(new JobDataMap(dic as IDictionary<string, object>))                  
                  .Build();

                var triggerBuilder = TriggerBuilder.Create()
                    .WithIdentity(collector.GetType().Name, "group")
                    .WithPriority(collector.Priority)
                    .EndAt(collector.EndAt);

                if (collector.StartingTime.HasValue || collector.RandomTimeDelay.Ticks > 0) {
                    var startTime = DateTimeOffset.UtcNow;
                    if(collector.StartingTime.HasValue) {
                        startTime = collector.StartingTime.Value;
                    }
                    startTime = startTime.Add(collector.RandomTimeDelay);
                    triggerBuilder.StartAt(startTime);
                }
                else {
                    triggerBuilder.StartNow();
                }               

                var trigger = triggerBuilder.WithSimpleSchedule(x => x
                    .WithInterval(collector.PollingInterval)
                    .RepeatForever())
                .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
        }

        private static IConfigurationRoot LoadConfig() {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }
    }
}
