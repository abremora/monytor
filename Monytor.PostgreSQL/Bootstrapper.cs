using Autofac;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL {
    public static class Bootstrapper {
        public static void SetupDatabaseAndRegisterRepositories(ContainerBuilder containerBuilder, string connectionString) {
            var documentStore = DocumentStore.For(_ => {
                _.Connection(connectionString);
            });

            containerBuilder.RegisterInstance(documentStore).As<IDocumentStore>();
            containerBuilder.RegisterType<SeriesRepository>().As<ISeriesRepository>();
            containerBuilder.RegisterType<DashboardRepository>().As<IDashboardRepository>();
            containerBuilder.RegisterType<CollectorConfigRepository>().As<ICollectorConfigRepository>();
            containerBuilder.RegisterType<BulkRepository>().As<IBulkRepository>();
        }

        public static void SetupDatabaseAndRegisterRepositories(IServiceCollection serviceCollection, string connectionString) {
            var documentStore = DocumentStore.For(_ => {
                _.Connection(connectionString);
            });

            serviceCollection.AddSingleton<IDocumentStore>(documentStore);
            serviceCollection.AddScoped<ISeriesRepository, SeriesRepository>();
            serviceCollection.AddScoped<IDashboardRepository, DashboardRepository>();
            serviceCollection.AddScoped<ICollectorConfigRepository, CollectorConfigRepository>();
        }


    }
}
