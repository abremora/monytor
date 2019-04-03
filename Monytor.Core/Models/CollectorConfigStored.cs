using Monytor.Core.Configurations;
using System;
using FluentValidation;
using Monytor.Core.Validator;

namespace Monytor.Core.Models {
    public class CollectorConfigStored : CollectorConfig {
        private static readonly CollectorConfigStoredValidator Validator = new CollectorConfigStoredValidator();
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string SchedulerAgentId { get; set; }

        public static string CreateId() {
            return nameof(CollectorConfigStored) + "/" + Guid.NewGuid();
        }

        public void ValidateAndThrow() {
            Validator.ValidateAndThrow(this);
        }
    }
}