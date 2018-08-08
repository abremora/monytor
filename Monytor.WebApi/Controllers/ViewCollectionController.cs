using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Monytor.Core.Services;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class ViewCollectionController : Controller
    {
        private readonly IViewCollectionService _collectorService;

        public ViewCollectionController(IViewCollectionService collectorService) {
            _collectorService = collectorService;
        }

        [HttpGet("{id}")]
        public ViewCollection Get(string id) {
            return _collectorService.Get(id);
        }

        [HttpGet]
        public IEnumerable<ViewCollection> Index() {
            return _collectorService.GetOverview();
        }

        [HttpPost]
        public IActionResult Set([FromBody]ViewCollection config) {
            if (config == null) return BadRequest();
            _collectorService.Set(config);

            return Ok();
        }
    }
}
