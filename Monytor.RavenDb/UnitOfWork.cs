using Monytor.Core.Repositories;
using Raven.Client;

namespace Monytor.RavenDb {

    public class UnitOfWork : IUnitOfWork {
        private bool disposedValue = false;
        private IDocumentSession _session;

        internal IDocumentStore Store { get; }
        public ISession Session { get; set; }

        public UnitOfWork(IDocumentStore store) {
            Store = store;
        }

        public ISession OpenSession() {
            if(Session == null) {
                _session = Store.OpenSession();
                Session = new RavenDbSession(_session);
            }
            return Session;
        }

        public void SaveChanges() {
            _session.SaveChanges();
        }       

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _session?.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
    }
}
