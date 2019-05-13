using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Monytor.RavenDb {
    public class RavenHelper {
        public static DocumentStore CreateStore(string url, string databaseName) {
            var documentStore = new DocumentStore() {
                Url = url,
                DefaultDatabase = databaseName,
                Conventions = new DocumentConvention() {
                    FindTypeTagName = t => t.Name                    
                }
            };

            documentStore.Initialize();
            return documentStore;
        }

        public static DocumentStore CreateStore(string connectionString) {
            var optsBuilder = ConnectionStringParser<RavenConnectionStringOptions>
                .FromConnectionString(connectionString);
            optsBuilder.Parse();

            var documentStore = new DocumentStore() {
                Url = optsBuilder.ConnectionStringOptions.Url,
                DefaultDatabase = optsBuilder.ConnectionStringOptions.DefaultDatabase,
                Conventions = new DocumentConvention() {
                    FindTypeTagName = t => t.Name
                }
            };

            documentStore.Initialize();
            return documentStore;
        }
    }
}
