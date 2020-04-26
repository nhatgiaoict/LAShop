using LACoreApp.Data.SeoStructureData.Common;
using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData
{
    public class WebsiteStructureData
    {
        [JsonProperty("@context")]
        public string Context { get; set; }
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("alternateName")]
        public string AlternateName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("potentialAction")]
        public PotentialAction PotentialAction { get; set; }
    }
}
