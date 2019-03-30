using Monytor.Core.Configurations;
using System;
using FluentValidation;
using Monytor.Implementation.Collectors.Sql;

namespace Monytor.Implementation.Collectors {

    public class RestApiCollector : Collector {
        private static readonly RestApiCollectorValidator Validator = new RestApiCollectorValidator();

        public Uri RequestUri { get; set; }
        public string JsonPath { get; set; }
        public string TagName { get; set; }
        public override string GroupName { get; set; } = "RestApi";

        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}