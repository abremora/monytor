using Autofac;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL {
    public static class Bootstrapper {
        public static void SetupDatabaseAndRegisterRepositories(ContainerBuilder containerBuilder, string connectionString) {
            var documentStore = DocumentStore.For(configure => {
                configure.Connection(connectionString);
            });
            
            containerBuilder.RegisterInstance(documentStore).As<IDocumentStore>();
            containerBuilder.RegisterType<SeriesQueryRepository>().As<ISeriesQueryRepository>();
            containerBuilder.RegisterType<BulkRepository>().As<IBulkRepository>();
        }

        public static void SetupDatabaseAndRegisterRepositories(IServiceCollection serviceCollection, string connectionString) {
            var documentStore = DocumentStore.For(configure => {
                configure.Connection(connectionString);
            });

            serviceCollection.AddSingleton<IDocumentStore>(documentStore);
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ISeriesRepository, SeriesRepository>();
            serviceCollection.AddScoped<ISeriesQueryRepository, SeriesQueryRepository>();
            serviceCollection.AddScoped<IDashboardRepository, DashboardRepository>();
            serviceCollection.AddScoped<ICollectorConfigRepository, CollectorConfigRepository>();
        }
    }
}
