using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Monytor.Core.Configurations;
using Newtonsoft.Json;

namespace Monytor.Domain.Services {
    public class CollectorFileConfigCreator  {
        public string ConfigFileName { get; }

        public CollectorFileConfigCreator(string configFileName = "collectorconfig.json") {
            ConfigFileName = configFileName;
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

        public bool HasConfig() {
            return File.Exists(GetConfigPath());
        }

        public void WriteConfig(object config, string configPath) {
            var content = JsonConvert.SerializeObject(config, JsonSerializerSettings());
            File.WriteAllText(configPath, content);
        }

        public string GetConfigPath() {
            var directory = GetEntryAssemblyDirectoryPath();
            return Path.Combine(directory, ConfigFileName);
        }

        public static JsonSerializerSettings JsonSerializerSettings() {
            return new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public static IEnumerable<T> LoadAll<T>()
           where T : class {
            var instances = Implementation.ImplementationTypeLoader.LoadAllConcreteTypesOf<T>()
                .Select(x => Activator.CreateInstance(x) as T);
            return instances;
        }

        private static string GetEntryAssemblyDirectoryPath() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}