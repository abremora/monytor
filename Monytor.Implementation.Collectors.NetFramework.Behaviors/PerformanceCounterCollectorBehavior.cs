using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Monytor.Implementation.Collectors.NetFramework {
    public class PerformanceCounterCollectorBehavior : CollectorBehavior<PerformanceCounterCollector> {
        public override IEnumerable<Series> Run(Collector collector) {
            var collectorTyped = collector as PerformanceCounterCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;
            var machineName = ".";
            if (!string.IsNullOrEmpty(collectorTyped.MachineName))
                machineName = collectorTyped.MachineName;

            float value = 0;
            var categoryWithoutInstance = collectorTyped.Category?.Split('(')[0];
            if (PerformanceCounterCategory.Exists(categoryWithoutInstance) && 
                PerformanceCounterCategory.CounterExists(collectorTyped.Counter, categoryWithoutInstance)) {
                var perfCounter = new PerformanceCounter(collectorTyped.Category, 
                    collectorTyped.Counter, 
                    !string.IsNullOrWhiteSpace(collectorTyped.Instance) ? collectorTyped.Instance : string.Empty,
                    machineName);
                // The method nextValue() always returns a 0 value on the first call. 
                // So you have to call this method a second time.
                value = perfCounter.NextValue();
                // Wait 1 sec to get accurate values as suggested here:
                // https://stackoverflow.com/questions/2181828/why-the-cpu-performance-counter-kept-reporting-0-cpu-usage
                Thread.Sleep(1000);
                value = perfCounter.NextValue();
            }

            var tag = $"{collectorTyped.Category}/{collectorTyped.Counter}";
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
