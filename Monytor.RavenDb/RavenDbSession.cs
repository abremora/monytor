using Monytor.Core.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {
    public class RavenDbSession : ISession {
        private readonly IDocumentSession _documentSession;

        public RavenDbSession(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }

        public T Load<T>(string id) {
            return _documentSession.Load<T>(id);
        }

        public T Load<T>(int id) {
            return _documentSession.Load<T>(id);
        }

        public void Store<T>(T item) {
            _documentSession.Store(item);
        }
    }
}
