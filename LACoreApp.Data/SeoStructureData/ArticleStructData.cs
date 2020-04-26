using LACoreApp.Data.SeoStructureData.Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace LACoreApp.Data.SeoStructureData
{
    public class ArticleStructData
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("mainEntityOfPage")]
        public MainEntityOfPage MainEntityOfPage { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("datePublished")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatePublished { get; set; }

        [JsonProperty("dateModified")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }
    }
}