using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokemonAPI
{
    public class TranslateModel
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        [JsonPropertyName("translated")]
        public string TranslatedText { get; set; }

        [JsonPropertyName("text")]
        public string OriginalText { get; set; }

        [JsonPropertyName("translation")]
        public string Translation { get; set; }
    }

}
