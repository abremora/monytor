using Marten;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL {

    public class UnitOfWork : IUnitOfWork {
        private bool disposedValue = false;
        internal IDocumentSession DirtyTrackedSession { get; private set; }

        internal IDocumentStore Store { get; }
        public ISession Session { get; set; }

        public UnitOfWork(IDocumentStore store) {
            Store = store;
        }

        public ISession OpenSession() {
            if(Session == null) {
                DirtyTrackedSession = Store.DirtyTrackedSession();
                Session = new MartenSession(DirtyTrackedSession);
            }
            return Session;
        }

        public void SaveChanges() {
            DirtyTrackedSession.SaveChanges();
        }       

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    DirtyTrackedSession?.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
    }
}
