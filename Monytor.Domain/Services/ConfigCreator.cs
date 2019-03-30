using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Monytor.Domain.Services {
    public abstract class ConfigCreator {
        public abstract string ConfigFileName { get; }

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
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var instances = loadedAssemblies.SelectMany(s => s.ExportedTypes)
                .Where(p => typeof(T).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as T);
            return instances;
        }

        private static string GetEntryAssemblyDirectoryPath() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}