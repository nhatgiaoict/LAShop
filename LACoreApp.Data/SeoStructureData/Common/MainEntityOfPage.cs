using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class MainEntityOfPage
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }
    }
}