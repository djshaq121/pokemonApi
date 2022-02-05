using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokemonAPI
{

    public class PokemonModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntries> FlavorTexts { get; set; }

        public string GetDescriptionByLangauge(string language)
        {
            if (FlavorTexts == null || !FlavorTexts.Any())
                return "";

            return FlavorTexts.FirstOrDefault(x => x?.Language?.Name == language)?.Description;
        }
    }

    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
    
    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class FlavorTextEntries
    {
        [JsonPropertyName("flavor_text")]
        public string Description { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }

}
