using Monytor.Core.Configurations;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Monytor.Startup {
    public class CollectorConfigCreator : ConfigCreator {
        private string _configFileName;
        public override string ConfigFileName => _configFileName;

        public CollectorConfigCreator(string configFileName = "collectorconfig.json") {
            _configFileName = configFileName;
        }

        public void CreateDefaultConfig() {
            var instances = LoadAll<Collector>().ToList();
            var verifiers = LoadAll<Verifier>().ToList();
            var notifications = LoadAll<Notification>().ToList();

            foreach (var v in verifiers) {
                v.Notifications.AddRange(notifications.Select(x => x.Id));
            }

            foreach (var c in instances) {
                c.Verifiers.AddRange(verifiers);
            }

            var config = new CollectorConfig {
                Collectors = instances,
                Notifications = notifications
            };

            string configPath = GetConfigPath();
            WriteConfig(config, configPath);
        }

        public CollectorConfig LoadConfig() {
            var collectorConfig = GetConfigPath();
            var content = File.ReadAllText(collectorConfig);
            return JsonConvert.DeserializeObject<CollectorConfig>(content, JsonSerializerSettings());
        }
    }
}