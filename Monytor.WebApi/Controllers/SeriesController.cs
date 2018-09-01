using System;
using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class SeriesController : Controller
    {
        private readonly ICollectorService _collectorService;

        public SeriesController(ICollectorService collectorService) {
            _collectorService = collectorService;
        }

        [HttpGet("{id}")]
        public Series Get(int id) {
            return _collectorService.GetSerie(id);
        }

        [HttpGet("{start}/{end}/{group}/{tag}")]
        public IEnumerable<Series> Get(DateTime start, DateTime end, string group, string tag, string meanValueType = null) {
            var query = new SeriesQuery {
                Start = start,
                End = end,
                Group = group,
                Tag = tag,
                MaxValues = 1024,
                OrderBy = Ordering.Ascending,
                MeanValueType = meanValueType
            };
            return _collectorService.GetSeries(query);
        }

        [HttpGet]
        public List<KeyValuePair<string, IEnumerable<string>>> GroupValueSummary() {
            return _collectorService.GetGroupValueSummary().ToList();
        }

        [HttpPost]
        public void Set([FromBody]Series series) {
            _collectorService.Set(series);
        }
    }
}
