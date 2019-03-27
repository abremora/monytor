using Monytor.Core.Repositories;
using Raven.Client;
using Raven.Client.Document;
using System;

namespace Monytor.RavenDb.Repositories {
    public class BulkRepository : IBulkRepository {
        private readonly IDocumentStore _store;
        private BulkInsertOperation _currentBulkOperation;

        public BulkRepository(IDocumentStore store) {
            _store = store;
        }

        public IDisposable BeginBulkInsert() {
            return _currentBulkOperation = _store.BulkInsert();
        }

        public void Store<TDocument>(TDocument entity) {
            _currentBulkOperation.Store(entity);
        }
    }
}
