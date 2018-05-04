using Monytor.Core.Models;
using System;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface ISerieRepository {
        Dictionary<string, IEnumerable<string>> GetGroupValueSummary();
        Serie GetSerie(int id);
        IEnumerable<Serie> GetSeries(SerieQuery query);
    }

    public interface IUnitOfWork : IDisposable {
        ISession OpenSession();
        void SaveChanges();
    }
}       