using Monytor.Collectors;
using Monytor.Core.Configurations;
using Monytor.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Monytor.Setup {
    internal class ConfigCreator {
        public const string ConfigFileName = "collectorconfig.json";

        public static void CreateDeaultCollectorConfig() {
            IEnumerable<Collector> instances = LoadAllCollectors();

            var config = new CollectorConfig {
                Collectors = instances.ToList()
            };

            var directoy = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var collectorConfig = Path.Combine(directoy, ConfigFileName);

            var content = JsonConvert.SerializeObject(config, JsonSerializerSettings());
            File.WriteAllText(collectorConfig, content);
            Logger.Info($"{ConfigFileName} was created.");
        }

        public static CollectorConfig LoadCollectorConfig() {
            var collectorConfig = Path.Combine(".", ConfigFileName);
            var content = File.ReadAllText(collectorConfig);
            return JsonConvert.DeserializeObject<CollectorConfig>(content, JsonSerializerSettings());
        }

        public static bool HasConfig() {
            return File.Exists(Path.Combine(".", ConfigFileName));
        }

        private static JsonSerializerSettings JsonSerializerSettings() {
            return new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        private static IEnumerable<Collector> LoadAllCollectors() {
            // Hack: Bind DLL with collectors
            new SystemInformationCollector();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var instances = loadedAssemblies.SelectMany(s => s.GetTypes())
                .Where(p => typeof(Collector).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as Collector);
            return instances;
        }
    }
}