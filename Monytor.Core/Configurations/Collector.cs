﻿using System;
using System.Collections.Generic;
using FluentValidation;
using Monytor.Core.Validator;

namespace Monytor.Core.Configurations {

    public abstract class Collector {
        private static readonly CollectorValidator Validator = new CollectorValidator();

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public abstract string GroupName { get; set; }
        public TimeSpan StartingTimeDelay { get; set; }
        public TimeSpan RandomTimeDelay { get; set; }
        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromHours(1);
        public bool OverlappingRecurring { get; set; }
        public DateTimeOffset? StartingTime { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public int Priority { get; set; } = 3;

        public List<Verifier> Verifiers { get; set; } = new List<Verifier>();

        public string CreateId() {
            return $"{GetType().Name}/{Guid.NewGuid()}";
        }

        public virtual void ValidateAndThrow() {
            Validator.ValidateAndThrow(this);
        }
    }
}
