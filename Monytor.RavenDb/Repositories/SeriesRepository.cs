using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.RavenDb {
    public class SeriesRepository : ISeriesRepository {
        private readonly UnitOfWork _unitOfWork;

        public SeriesRepository(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Series GetSeries(int id) {
            return _unitOfWork.Session.Load<Series>(id);
        }

        public void Store(Series series) {
            _unitOfWork.Session.Store(series);
        }
    }
}
