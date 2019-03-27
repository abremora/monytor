using System;
using System.Collections.Generic;

namespace Monytor.PostgreSQL {
    public class BulkInsertOperation : IDisposable {
        private bool _disposedValue = false;

        internal event EventHandler<List<object>> OnDispose;               

        internal List<object> DocumentsToInsert { get; } = new List<object>();
        
        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                OnDispose?.Invoke(this, this.DocumentsToInsert);
                if (disposing) {
                }
                _disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
