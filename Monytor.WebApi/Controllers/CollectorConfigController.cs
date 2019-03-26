using System;
using System.Collections.Generic;
using System.Linq;
using Monytor.Core.Models;
using Monytor.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Monytor.Contracts.CollectorConfig;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class CollectorConfigController : Controller
    {
        private readonly ICollectorConfigService _collectorConfigService;

        public CollectorConfigController(ICollectorConfigService collectorConfigService) {
            _collectorConfigService = collectorConfigService;
        }

        [HttpGet("{id}")]
        public ActionResult<CollectorConfigStored> Get(string id) {
            return Ok(_collectorConfigService.Get(id));
        }

        [HttpPost]
        public ActionResult<string> Create([FromBody]CreateCollectorConfigCommand command) {
            var id = _collectorConfigService.Create(command);
            return Ok(id);
        }

        [HttpPost("{collectorConfigId}/AddSqlCountCollector")]
        public ActionResult<string> AddSqlCountCollector(string collectorConfigId, [FromBody] AddSqlCountCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(collectorConfigId, command);
            return Ok();
        }
    }   
}
