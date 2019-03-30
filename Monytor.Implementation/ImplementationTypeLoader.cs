using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Monytor.Implementation {
    public static class ImplementationTypeLoader {

        private static readonly Lazy<List<Assembly>> _implementationAssemblies = new Lazy<List<Assembly>>(
            valueFactory: LoadImplementationAssemblies
            );

        public static Type LoadBehavior(Type behaviorType, Type instance) {
            var constructedListType = behaviorType.MakeGenericType(instance);
            return LoadAllConcreteTypesOf(constructedListType).SingleOrDefault();
        }

        public static IEnumerable<Type> LoadAllConcreteTypesOf(Type type) {
            var types = _implementationAssemblies.Value.SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass
                    && !p.IsAbstract
                    && type.IsAssignableFrom(p));
            return types;
        }

        private static string GetEntryAssemblyDirectoryPath() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        private static List<Assembly> LoadImplementationAssemblies() {
            var alreadyLoadedImplemenationAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(w => w.FullName.StartsWith("Monytor.Implementation")).ToList();

            var implementationAssemblyFiles = Directory.GetFiles(GetEntryAssemblyDirectoryPath(), "Monytor.Implementation*.dll", SearchOption.TopDirectoryOnly);
            var implementationAssemblies = new List<Assembly>();
            foreach (var assemblyFile in implementationAssemblyFiles) {
                var assembly = alreadyLoadedImplemenationAssemblies
                    .FirstOrDefault(f => f.Location == assemblyFile);
                if (assembly == null) {
                    assembly = Assembly.LoadFrom(assemblyFile);
                }
                implementationAssemblies.Add(assembly);
            }
            return implementationAssemblies;
        }
    }
}
