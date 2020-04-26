using System.Collections.Generic;
using LACoreApp.Data.SeoStructureData.Common;
using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData
{
    public class BreadCrumbListStructureData
    {
        [JsonProperty("@context")]
        public string Context { get; set; }
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("itemListElement")]
        public List<ItemListElement> ItemListElements { get; set; }
    }
}
