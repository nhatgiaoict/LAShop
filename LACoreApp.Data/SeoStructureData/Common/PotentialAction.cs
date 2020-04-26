using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class PotentialAction
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("query-input")]
        public string QueryInput { get; set; }
    }
}
