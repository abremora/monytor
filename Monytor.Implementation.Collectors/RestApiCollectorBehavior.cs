using Microsoft.Extensions.Logging;
using Monytor.Core.Configurations;
using Monytor.Core.Models;
using Monytor.Infrastructure;
using Monytor.Infrastructure.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Monytor.Implementation.Collectors {
    public class RestApiCollectorBehavior : CollectorBehavior<RestApiCollector>,
        IDisposable {
        static HttpClient _client = new HttpClient();
        static Interpreter _interpreter = new Interpreter();

        private readonly ILogger<RestApiCollectorBehavior> _logger;

        public RestApiCollectorBehavior(ILogger<RestApiCollectorBehavior> logger) {
            _logger = logger;
        }

        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as RestApiCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            string content = "";

            var replacement = _interpreter.ReplacePlaceholder(collectorTyped.RequestUri.OriginalString);
            var requestUriPlaceholder = new Uri(replacement);

            var response = _client.GetAsync(requestUriPlaceholder).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode) {
                content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else {
                _logger.LogWarning($"'{requestUriPlaceholder}' returns: {response.StatusCode}");
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(collectorTyped.JsonPath)) {
                var json = JToken.Parse(content);
                var results = json.SelectTokens(collectorTyped.JsonPath);

                foreach (var result in results) {
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
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    _client?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}