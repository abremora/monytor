using Microsoft.AspNetCore.Mvc;
using Monytor.Contracts.CollectorConfig;
using Monytor.Core.Models;
using Monytor.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class CollectorConfigController : Controller {
        private readonly ICollectorConfigService _collectorConfigService;

        public CollectorConfigController(ICollectorConfigService collectorConfigService) {
            _collectorConfigService = collectorConfigService;
        }

        [HttpGet("{*collectorConfigId}")]
        public async Task<ActionResult<CollectorConfigStored>> GetCollectorConfig(string collectorConfigId) {
            return Ok(await _collectorConfigService.GetCollectorConfigAsync(Uri.UnescapeDataString(collectorConfigId)));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateCollectorConfig([FromBody]CreateCollectorConfigCommand command) {
            var id = await _collectorConfigService.CreateCollectorConfigAsync(command);
            return Ok(id);
        }

        [HttpDelete("{*collectorConfigId}")]
        public async Task<ActionResult> DeleteCollectorConfigAsync(string collectorConfigId) {
            await _collectorConfigService.DeleteCollectorConfigAsync(Uri.UnescapeDataString(collectorConfigId));
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddSqlCountCollector")]
        public async Task<ActionResult<string>> AddSqlCountCollector(string collectorConfigId, [FromBody] AddSqlCountCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbStartingWithCollector")]
        public async Task<ActionResult<string>> AddRavenDbStartingWithCollector(string collectorConfigId, [FromBody] AddRavenDbStartingWithCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbCollectionCollector")]
        public async Task<ActionResult<string>> AddRavenDbCollectionCollector(string collectorConfigId, [FromBody] AddRavenDbCollectionCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRavenDbAllCollectionCollector")]
        public async Task<ActionResult<string>> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddRavenDbAllCollectionCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddRestApiCollector")]
        public async Task<ActionResult<string>> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddRestApiCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddSystemInformationCollector")]
        public async Task<ActionResult<string>> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddSystemInformationCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpPost("{collectorConfigId}/AddPerformanceCounterCollector")]
        public async Task<ActionResult<string>> AddRavenDbAllCollectionCollector(string collectorConfigId, [FromBody] AddPerformanceCounterCollectorToConfigCommand command) {
            await _collectorConfigService.AddCollectorAsync(Uri.UnescapeDataString(collectorConfigId), command);
            return Ok();
        }

        [HttpDelete("{collectorConfigId}/{*collectorId}")]
        public async Task<ActionResult> DeleteCollectorConfigAsync(string collectorConfigId, string collectorId) {
            await _collectorConfigService.DeleteCollectorAsync(Uri.UnescapeDataString(collectorConfigId), Uri.UnescapeDataString(collectorId));
            return Ok();
        }

    }
}
