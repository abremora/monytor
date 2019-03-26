using Monytor.Core.Models;
using Monytor.Core.Repositories;
using Monytor.Core.Services;
using System;
using System.Collections.Generic;

namespace Monytor.Domain.Factories {
    public class SeriesService : ISeriesService {
        private readonly ISeriesRepository _repository;

        public SeriesService(ISeriesRepository repository) {
            _repository = repository;
        }

        public Dictionary<string, IEnumerable<string>> GetGroupValueSummary() {
            return _repository.GetGroupValueSummary();
        }

        public Series GetSerie(int id) {
            return _repository.GetSeries(id);
        }

        public IEnumerable<Series> GetSeries(SeriesQuery query) {
            var unescapedQuery = new SeriesQuery {
                Start = query.Start,
                End = query.End,
                MaxValues = query.MaxValues,
                OrderBy = query.OrderBy,
                Group = Uri.UnescapeDataString(query.Group),
                Tag = Uri.UnescapeDataString(query.Tag),
                MeanValueType = query.MeanValueType == null ? "" : query.MeanValueType
            };

            switch (unescapedQuery.MeanValueType.Trim().ToLower()) {
                case "day":
                    return _repository.GetSeriesByDayMean(unescapedQuery);
                case "hour":
                    return _repository.GetSeriesByHourMean(unescapedQuery);
                default:
                    return _repository.GetSeries(unescapedQuery);
            }
        }

        public void Create(Series series) {
            _repository.Store(series);
        }
    }
}