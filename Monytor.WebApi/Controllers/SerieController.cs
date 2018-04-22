using System;
using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class SerieController : Controller
    {
        private readonly ICollectorService _collectorService;

        public SerieController(ICollectorService collectorService) {
            _collectorService = collectorService;
        }
   
        // GET api/values/5
        [HttpGet("{id}")]
        public Serie Get(int id)
        {
            return _collectorService.GetSerie(id);
        }

        // GET api/user/firstname/lastname/address
        [HttpGet("{start}/{end}/{group}/{tag}")]
        public IEnumerable<Serie> Get(DateTime start, DateTime end, string group, string tag) {
            var query = new SerieQuery {
                Start = start,
                End = end,
                Group = group,
                Tag = tag
            };
            return _collectorService.GetSeries(query);
        }

        [HttpGet]
        public List<KeyValuePair<string, IEnumerable<string>>> GroupValueSummary() {
            return _collectorService.GetGroupValueSummary().ToList();
        }
    }
}
