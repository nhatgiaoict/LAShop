using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class Logo
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
