using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Monytor.Collectors {
    public class SystemInformationCollector : Collector {
        public override string GroupName { get; set; }

        public SystemInformationCollector() {
            GroupName = "SystemInformation";
        }

        public override IEnumerable<Serie> Run() {
            var currentTime = DateTime.UtcNow;

            var processes = Process.GetProcesses();
            var mem = processes.Sum(x => x.WorkingSet64);

            var serie = new Serie {
                Id = Serie.CreateId("TotalMemory", GroupName, currentTime),
                Tag = "TotalMemory",
                Group = GroupName,
                Time = currentTime,
                Value = mem.ToString()
            };

            yield return serie;
        }
    }
}
