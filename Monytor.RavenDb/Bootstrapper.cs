using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Monytor.Core.Repositories;
using Monytor.RavenDb.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {
    public static class Bootstrapper {
        public static void SetupDatabaseAndRegisterRepositories(ContainerBuilder containerBuilder, string databaseUrl, string databaseName) {
            var documentStore = RavenHelper.CreateStore(databaseUrl, databaseName);

            new SeriesIndex().SideBySideExecute(documentStore);
            new SeriesByDayIndex().SideBySideExecute(documentStore);
            new SeriesByHourIndex().SideBySideExecute(documentStore);
            new TagGroupMapReduceIndex().SideBySideExecute(documentStore);
            
            containerBuilder.RegisterInstance(documentStore)
                   .As<IDocumentStore>();
            containerBuilder.RegisterType<SeriesRepository>()
                    .As<ISeriesRepository>();
            containerBuilder.RegisterType<DashboardRepository>()
                    .As<IDashboardRepository>();
            containerBuilder.RegisterType<BulkRepository>()
                    .As<IBulkRepository>();
        }

        public static void SetupDatabaseAndRegisterRepositories(IServiceCollection serviceCollection, string databaseUrl, string databaseName) {
            var documentStore = RavenHelper.CreateStore(databaseUrl, databaseName);
            serviceCollection.AddSingleton<IDocumentStore>(documentStore);
            serviceCollection.AddScoped<ISeriesRepository, SeriesRepository>();
            serviceCollection.AddScoped<IDashboardRepository, DashboardRepository>();
        }
    }
}
