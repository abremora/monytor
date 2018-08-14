using Microsoft.Extensions.Logging;
using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Monytor.Implementation.Collectors {
    public class RestApiCollectorBehavior : CollectorBehavior<RestApiCollector>, 
        IDisposable {
        static HttpClient _client = new HttpClient();

        private readonly ILogger<RestApiCollectorBehavior> _logger;

        public RestApiCollectorBehavior(ILogger<RestApiCollectorBehavior> logger) {
            _logger = logger;
        }

     

        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as RestApiCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            string content = "";

            HttpResponseMessage response = _client.GetAsync(collectorTyped.RequestUri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode) {
                content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else {
                _logger.LogWarning($"'{collectorTyped.RequestUri}' returns: {response.StatusCode}");
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(collectorTyped.JsonPath)) {
                var json = JToken.Parse(content);
                var results = json.SelectTokens(collectorTyped.JsonPath);

                foreach(var result in results) {
                    var serieParsed = new Series {
                        Id = Series.CreateId(collectorTyped.TagName, collectorTyped.GroupName, currentTime),
                        Tag = collectorTyped.TagName,
                        Group = collectorTyped.GroupName,
                        Time = currentTime,
                        Value = result.Value<string>()
                    };

                    yield return serieParsed;
                }
            }
            else {
                var serie = new Series {
                    Id = Series.CreateId(collectorTyped.RequestUri.ToString(), collectorTyped.GroupName, currentTime),
                    Tag = collectorTyped.RequestUri.ToString(),
                    Group = collectorTyped.GroupName,
                    Time = currentTime,
                    Value = content
                };

                yield return serie;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _client?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);            
        }
        #endregion
    }
}