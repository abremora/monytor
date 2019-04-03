﻿using Marten;
using Monytor.Core.Repositories;

namespace Monytor.PostgreSQL {
    public class MartenSession : ISession {
        private readonly IDocumentSession _documentSession;

        public MartenSession(Marten.IDocumentSession documentSession) {
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

        public void Delete<T>(T item) {
            _documentSession.Delete(item);
        }

        public void Delete(string id) {
            _documentSession.Delete(id);
        }

        public void Delete(int id) {
            _documentSession.Delete(id);
        }
    }
}
