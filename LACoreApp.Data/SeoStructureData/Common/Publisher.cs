using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class Publisher
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public Logo Logo { get; set; }
    }
    
}
