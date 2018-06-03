using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monytor.NetFramework.Implementation {
    public class PerformanceCounterCollectorBehavior : CollectorBehavior<PerformanceCounterCollector> {
        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as PerformanceCounterCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;

            float value = 0;
            var categoryWithoutInstance = collectorTyped.Category?.Split('(')[0];
            if (PerformanceCounterCategory.Exists(categoryWithoutInstance) && 
                PerformanceCounterCategory.CounterExists(collectorTyped.Name, categoryWithoutInstance)) {
                var perfCounter = new PerformanceCounter(collectorTyped.Category, collectorTyped.Name, 
                    !string.IsNullOrWhiteSpace(collectorTyped.Instance) ? collectorTyped.Instance : string.Empty);
                // The method nextValue() always returns a 0 value on the first call. 
                // So you have to call this method a second time.
                value = perfCounter.NextValue();
                value = perfCounter.NextValue();
            }

            var tag = $"{collectorTyped.Category}/{collectorTyped.Name}";
            var series = new Series {
                Id = Series.CreateId(tag, collectorTyped.GroupName, currentTime),
                Tag = tag,
                Group = collectorTyped.GroupName,
                Time = currentTime,
                Value = value.ToString()
            };

            yield return series;
        }
    }
}
