using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Monytor.Implementation.Collectors {
    public class SystemInformationCollectorBehavior : CollectorBehavior<SystemInformationCollector> {
        public override IEnumerable<Serie> Run(Collector collector) {
            var collectorTyped = collector as SystemInformationCollector;
            if (collectorTyped == null) yield return null;

            var currentTime = DateTime.UtcNow;

            var processes = Process.GetProcesses();
            var mem = processes.Sum(x => x.WorkingSet64);

            var serie = new Serie {
                Id = Serie.CreateId("TotalMemory", collectorTyped.GroupName, currentTime),
                Tag = "TotalMemory",
                Group = collectorTyped.GroupName,
                Time = currentTime,
                Value = mem.ToString()
            };

            yield return serie;
        }
    }
}
