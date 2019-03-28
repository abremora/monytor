﻿using System;
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
            return Ok(_collectorConfigService.Get(Uri.UnescapeDataString(id)));
        }

        [HttpPost]
        public ActionResult<string> Create([FromBody]CreateCollectorConfigCommand command) {
            var id = _collectorConfigService.Create(command);
            return Ok(id);
        }

        [HttpPost("{collectorConfigId}/AddSqlCountCollector")]
        public ActionResult<string> AddSqlCountCollector(string collectorConfigId, [FromBody] AddSqlCountCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbStartingWithCollector")]
        public ActionResult<string> AddRavenDbStartingWithCollector(string collectorConfigId, [FromBody] AddRavenDbStartingWithCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbCollectionCollector")]
        public ActionResult<string> AddRavenDbCollectionCollector(string collectorConfigId, [FromBody] AddRavenDbCollectionCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbAllCollectionCollector")]
        public ActionResult<string> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddRavenDbAllCollectionCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRestApiCollector")]
        public ActionResult<string> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddRestApiCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddSystemInformationCollector")]
        public ActionResult<string> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddSystemInformationCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddPerformanceCounterCollector")]
        public ActionResult<string> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddPerformanceCounterCollectorToConfigCommand command) {
            _collectorConfigService.AddCollector(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }
    }   
}
