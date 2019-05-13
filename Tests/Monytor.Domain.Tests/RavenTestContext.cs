using Raven.Client.Embedded;

namespace Monytor.Domain.Tests {
    public class RavenTestContext : Raven.Tests.Helpers.RavenTestBase {
        public void ConfigureTestStore(EmbeddableDocumentStore documentStore) {
            documentStore.Configuration.Storage.Voron.AllowOn32Bits = true;
        }
    }
}
