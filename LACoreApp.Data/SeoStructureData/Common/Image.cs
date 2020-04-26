using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class Image
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("with")]
        public int With { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}