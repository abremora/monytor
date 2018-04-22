﻿using Monytor.Core.Models;
using System;
using System.Collections.Generic;

namespace Monytor.Core.Configurations {
    public abstract class Collector {
        public abstract string GroupName { get; set; }
        public TimeSpan StartingTimeDelay { get; set; }
        public TimeSpan RandomTimeDelay { get; set; }
        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromHours(1);
        public bool OverlappingReccuring { get; set; }
        public DateTimeOffset? StartingTime { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public int Priority { get; set; } = 3;
        
        public abstract IEnumerable<Serie> Run();
    }
}