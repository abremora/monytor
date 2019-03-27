using Marten;
using Monytor.Core.Repositories;
using System;
using System.Collections.Generic;

namespace Monytor.PostgreSQL {
    public class BulkRepository : IBulkRepository {
        private readonly IDocumentStore _store;
        private BulkInsertOperation _currentBulkOperation;

        public BulkRepository(IDocumentStore store) {
            _store = store;
        }

        public IDisposable BeginBulkInsert() {
            if(_currentBulkOperation != null) {
                throw new InvalidOperationException("The bulk insert operation can not be started twice.");
            }

            _currentBulkOperation = new BulkInsertOperation();
            _currentBulkOperation.OnDispose += OnCurrentBulkOperationDisposing;
            
            return _currentBulkOperation;
        }     

        public void Store<TDocument>(TDocument entity) {
            _currentBulkOperation.DocumentsToInsert.Add(entity);
        }

        private void OnCurrentBulkOperationDisposing(object sender, List<object> documentsToInsert) {
            _store.BulkInsert(_currentBulkOperation.DocumentsToInsert, BulkInsertMode.InsertsOnly);
        }
    }
}
