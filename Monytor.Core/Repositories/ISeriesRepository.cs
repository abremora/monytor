using Monytor.Core.Models;
using System.Collections.Generic;

namespace Monytor.Core.Repositories {
    public interface ISeriesRepository {
        Series GetSeries(int id);
        void Store(Series series);
    }
}       