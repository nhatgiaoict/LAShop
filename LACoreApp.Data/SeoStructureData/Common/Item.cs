using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LACoreApp.Data.SeoStructureData.Common
{
    public class Item
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
