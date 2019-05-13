using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Monytor.Core.Models;
using Monytor.Core.Services;

namespace Monytor.WebApi.Controllers {
    [Route("api/[controller]")]
    public class ViewCollectionController : Controller
    {
        private readonly IViewCollectionService _collectorService;

        public ViewCollectionController(IViewCollectionService collectorService) {
            _collectorService = collectorService;
        }

        [HttpGet("{*id}")]
        public Dashboard Get(string id) {
            return _collectorService.Get(Uri.UnescapeDataString(id));
        }

        [HttpGet]
        public IEnumerable<Dashboard> Index() {
            return _collectorService.GetOverview();
        }

        [HttpPost]
        public IActionResult Create([FromBody]Dashboard config) {
            if (config == null) return BadRequest();
            _collectorService.Create(config);

            return Ok();
        }
    }
}
