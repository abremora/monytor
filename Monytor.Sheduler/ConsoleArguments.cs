using CommandLine;

namespace Monytor.Sheduler {
    internal class ConsoleArguments {
        [Option( Default = false, HelpText = "Create a default config with settings for all collectors.")]
        public bool CreateDefaultConfig { get; set; }
    }
}
