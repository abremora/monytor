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

        [HttpGet("search")]
        public async Task<ActionResult<CollectorConfigSearchResult>> SearchCollectorConfig(
            [FromQuery] string searchTerms = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
            return Ok(await _collectorConfigService.SearchCollectorConfigAsync(searchTerms, page, pageSize));
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


        [HttpPost("{*collectorConfigId}")]
        public async Task<ActionResult> EditCollectorConfig(string collectorConfigId, [FromBody]EditCollectorConfigCommand command)
        {
            collectorConfigId = Uri.UnescapeDataString(collectorConfigId);
            if (collectorConfigId != command.Id)
            {
                return BadRequest();
            }
            await _collectorConfigService.EditCollectorConfigAsync(command);
            return Ok();
        }

        [HttpDelete("{*collectorConfigId}")]
        public async Task<ActionResult> DeleteCollectorConfigAsync(string collectorConfigId) {
            await _collectorConfigService.DeleteCollectorConfigAsync(Uri.UnescapeDataString(collectorConfigId));
            return Ok();
        }
    }
}
