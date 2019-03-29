using Autofac;
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
            return File.Exists(GetConfigPath());
        }

        public void WriteConfig(object config, string configPath) {
            var content = JsonConvert.SerializeObject(config, JsonSerializerSettings());
            File.WriteAllText(configPath, content);
        }

        public string GetConfigPath() {
            var directoy = GetEntryAssemblyDirectoryPath();
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

        public static Type LoadBehavior(Type behaviorType, Type instance) {
            var constructedListType = behaviorType.MakeGenericType(instance);
            return LoadAllConcreteTypesOf(constructedListType).Single();
        }

        internal static IEnumerable<Type> LoadAllConcreteTypesOf(Type type) {
            var implementationAssemblyFiles = System.IO.Directory.GetFiles(GetEntryAssemblyDirectoryPath(), "Monytor.Implementation*.dll", SearchOption.TopDirectoryOnly);
            var implementationAssembiles = new List<Assembly>();
            foreach (var assemblyFile in implementationAssemblyFiles) {
                implementationAssembiles.Add(Assembly.LoadFile(assemblyFile));
            }
            
            var types = implementationAssembiles.SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass
                    && !p.IsAbstract
                    && type.IsAssignableFrom(p));
            return types;
        }

        private static string GetEntryAssemblyDirectoryPath() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}