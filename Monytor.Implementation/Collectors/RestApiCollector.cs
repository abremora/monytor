using Monytor.Core.Configurations;
using System;

namespace Monytor.Implementation.Collectors {

    public class RestApiCollector : Collector {
        public Uri RequestUri { get; set; }
        public string JsonPath { get; set; }
        public string TagName { get; set; }
        public override string GroupName { get; set; }        

        public RestApiCollector() {
            GroupName = "RestApi";
        }        
    }
}