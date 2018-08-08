using System;
using System.Collections.Generic;
using System.Linq;
using Monytor.WebApi.Controllers;
using Raven.Client;

namespace Monytor.Domain.Services {
    public class ViewCollectionRepository : IViewCollectionRepository {
        private readonly IDocumentStore _store;

        public ViewCollectionRepository(IDocumentStore store) {
            _store = store;
        }

        public ViewCollection Load(string id) {
            using (var session = _store.OpenSession()) {
                return session.Load<ViewCollection>(id);
            }
        }

        public IEnumerable<ViewCollection> LoadOverview() {
            using (var session = _store.OpenSession()) {
                return session.Query<ViewCollection>()
                    .Take(1024);
            }
        }

        public void Save(ViewCollection config) {
            using (var session = _store.OpenSession()) {                
                session.Store(config);
                session.SaveChanges();
            }
        }
    }
}