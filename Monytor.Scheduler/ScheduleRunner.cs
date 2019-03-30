using Autofac;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors;
using Monytor.Startup;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Monytor.Domain.Services;

namespace Monytor.Scheduler {
    public abstract class ScheduleRunner {
        static IContainer _container;
        static ILogger _logger = null;
        static ManualResetEventSlim _manualReset = new ManualResetEventSlim(false);

        public async Task RunAsync(string[] args) {
            Console.WriteLine(CommandLine.Text.HeadingInfo.Default);
            Console.WriteLine(CommandLine.Text.CopyrightInfo.Default);
            Console.WriteLine();
            Console.WriteLine("Start application with '--help' for assistance.");
            Console.WriteLine();
            Console.WriteLine("Press <CTRL>+<c> to close the application.");
            Console.WriteLine();

            Console.CancelKeyPress += Console_CancelKeyPress;

            try {
                CreateLoggerForConsole();
                SetupBinder();

                var config = new CollectorConfigCreator();
                var parser = new Parser(x => x.CaseSensitive = false);
                var options = parser.ParseArguments<ConsoleArguments>(args);
                if (options.Tag == ParserResultType.NotParsed) {
                    goto End;
                }

                var parsedResult = options as Parsed<ConsoleArguments>;
                if (parsedResult.Value.CreateDefaultConfig) {
                    config.CreateDefaultConfig();
                    _logger.LogInformation("Default config was created");
                    goto End;
                }

                var appConfig = LoadConfigurationRoot();
                

                _container = await Bootstrapper.Setup(appConfig);
                if (!config.HasConfig()) {
                    _logger.LogWarning($"Config file '{config.ConfigFileName}' not found. Create default config.\nUse --help for further assistance.");
                    return;
                }

                await RunAsync();
            }
            catch (Exception ex) {
                _logger?.LogCritical(ex, "Unknown error");

                #if DEBUG
                    Console.ReadKey();
                #endif
            }

        End:
            Console.WriteLine("Bye!");
        }

        private static IConfigurationRoot LoadConfigurationRoot() {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .Build();
        }

        static async Task RunAsync() {
            SchedulerStartup scheduler = null;
            try {
                scheduler = _container.Resolve<SchedulerStartup>();

                await scheduler.ConfigScheduler();

                _logger.LogInformation("Scheduler started");
                _manualReset.Wait();
            }
            catch (Exception e) {
                _logger.LogError(e, "Scheduler error");
            }
            finally {
                scheduler.Dispose();
            }
        }

        protected virtual void SetupBinder() {
            new SystemInformationCollector();
        }

        private static void CreateLoggerForConsole() {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            _logger = loggerFactory.CreateLogger<ScheduleRunner>();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            _logger?.LogInformation("Application will be closed.");
            var scheduler = _container.Resolve<SchedulerStartup>();
            scheduler.Dispose();
            _container?.Dispose();
            _logger?.LogInformation("All resources were disposed.");
            _manualReset.Set();
        }
    }

}
