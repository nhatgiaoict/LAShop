using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class Author
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}