using Monytor.Core.Models;
using Monytor.Core.Repositories;
using System;

namespace Monytor.PostgreSQL {

    public class SeriesRepository : ISeriesRepository {
        private readonly UnitOfWork _unitOfWork;

        public SeriesRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        public Series GetSeries(int id) {
            return _unitOfWork.Session.Load<Series>(id);
        }

        public void Store(Series series) {
            _unitOfWork.Session.Store(series);
        }
    }
}
