using Raven.Client.Document;

namespace Monytor.Infrastructure {
    public class RavenHelper {
        public static DocumentStore CreateStore(string url, string databaseName) {
            var documentStore = new DocumentStore {
                Url = url,
                DefaultDatabase = databaseName,
                Conventions = new DocumentConvention() {
                    FindTypeTagName = t => t.Name                    
                }
            };

            documentStore.Initialize();
            return documentStore;
        }
    }
}
