using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Monytor.Core.Repositories;
using Monytor.RavenDb.Repositories;
using Raven.Client;
using Raven.Client.Document;

namespace Monytor.RavenDb {
    public static class Bootstrapper {
        public static void SetupDatabaseAndRegisterRepositories(ContainerBuilder containerBuilder, string databaseUrl, string databaseName) {
            var documentStore = SetupStore(databaseUrl, databaseName);

            containerBuilder.RegisterInstance(documentStore).As<IDocumentStore>();
            containerBuilder.RegisterType<SeriesQueryRepository>().As<ISeriesQueryRepository>();
            containerBuilder.RegisterType<BulkRepository>().As<IBulkRepository>();
        }

        public static void SetupDatabaseAndRegisterRepositories(IServiceCollection serviceCollection, string databaseUrl, string databaseName) {

            var documentStore = SetupStore(databaseUrl, databaseName);
            serviceCollection.AddSingleton<IDocumentStore>(documentStore);
            serviceCollection.AddScoped<ISeriesRepository, SeriesRepository>();
            serviceCollection.AddScoped<ISeriesQueryRepository, SeriesQueryRepository>();
            serviceCollection.AddScoped<IDashboardRepository, DashboardRepository>();
            serviceCollection.AddScoped<ICollectorConfigRepository, CollectorConfigRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static DocumentStore SetupStore(string databaseUrl, string databaseName) {
            var documentStore = RavenHelper.CreateStore(databaseUrl, databaseName);

            new SeriesIndex().SideBySideExecute(documentStore);
            new SeriesByDayIndex().SideBySideExecute(documentStore);
            new SeriesByHourIndex().SideBySideExecute(documentStore);
            new TagGroupMapReduceIndex().SideBySideExecute(documentStore);
            return documentStore;
        }
    }
}
