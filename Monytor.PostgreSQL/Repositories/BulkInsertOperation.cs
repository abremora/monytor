using System;
using System.Collections.Generic;

namespace Monytor.PostgreSQL {
    public class BulkInsertOperation : IDisposable {
        
        internal event EventHandler<List<object>> OnDispose;               

        internal List<object> DocumentsToInsert { get; } = new List<object>();

        public BulkInsertOperation() {
            
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                OnDispose?.Invoke(this, this.DocumentsToInsert);
                if (disposing) {
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
