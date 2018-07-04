using Autofac;
using CommandLine;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors;
using Monytor.Infrastructure;
using Monytor.NetFramework.Implementation;
using Monytor.Startup;
using System;
using System.Threading.Tasks;

namespace Monytor.NetFramework {
    class Program {
        static void Main(string[] args) {
            Logger.Text(CommandLine.Text.HeadingInfo.Default);
            Logger.Text(CommandLine.Text.CopyrightInfo.Default);
            Logger.NewLine();
            Logger.Text("Start application with '--help' for assistance.");
            Logger.NewLine();

            try {
                SetupBinder();
                var config = new CollectorConfigCreator();
                var parser = new Parser(x => x.CaseSensitive = false);
                var options = parser.ParseArguments<ConsoleArguments>(args)
                    .WithParsed(o => { if (o.CreateDefaultConfig) config.CreateDefaultConfig(); });

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
            SchedulerStartup scheduler = null;
            try {
                scheduler = container.Resolve<SchedulerStartup>();
                var collecotConfig = container.Resolve<CollectorConfig>();

                await scheduler.ConfigScheduler(collecotConfig);

                Logger.Info("Scheduler started");
                Logger.Text("Press <ENTER> to close the application");
                Logger.NewLine();
                Console.ReadLine();
            }
            catch (Exception e) {
                Logger.Error(e);
            }
            finally {
                scheduler.Dispose();
            }
        }

        private static void SetupBinder() {
            new SystemInformationCollector();
            new PerformanceCounterCollector();
        }
    }

    internal class ConsoleArguments {
        [Option(Default = false, HelpText = "Create a default config with settings for all collectors.")]
        public bool CreateDefaultConfig { get; set; }
    }
}
