using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class ItemListElement
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
        [JsonProperty("item")]
        public Item Item { get; set; }
    }
}
