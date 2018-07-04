using Autofac;
using Monytor.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Monytor.Startup {
    public abstract class ConfigCreator {
        public abstract string ConfigFileName { get; }

        public bool HasConfig() {
            return File.Exists(Path.Combine(".", ConfigFileName));
        }

        public void WriteConfig(object config, string configPath) {
            var content = JsonConvert.SerializeObject(config, JsonSerializerSettings());
            File.WriteAllText(configPath, content);
            Logger.Info($"{ConfigFileName} was created.");
        }

        public string GetConfigPath() {
            var directoy = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return Path.Combine(directoy, ConfigFileName);
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

        public static object LoadBehavior(Type behaviorType, Type instance) {
            var constructedListType = behaviorType.MakeGenericType(instance);
            return LoadAll(constructedListType).Single();
        }

        private static IEnumerable<object> LoadAll(Type type) {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var instances = loadedAssemblies.SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .Select(x => Activator.CreateInstance(x));
            return instances;
        }
    }
}