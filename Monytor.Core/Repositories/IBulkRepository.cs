using System;

namespace Monytor.Core.Repositories {
        public interface IBulkRepository {
        IDisposable BeginBulkInsert();
        void Store<TDocument>(TDocument entity);
    }
}