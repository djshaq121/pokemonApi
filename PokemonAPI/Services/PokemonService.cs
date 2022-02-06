using PokemonAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ITranslateService translateService;

        public PokemonService(IHttpClientFactory httpClient, ITranslateService translateService)
        {
            clientFactory = httpClient;
            this.translateService = translateService;
        }

        public async Task<PokemonModel> GetPokemonInformation(string pokemonName)
        {
            var url = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonName;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
           
            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                using (var result = await response.Content.ReadAsStreamAsync())
                {
                    var pokemonModel = await JsonSerializer.DeserializeAsync<PokemonModel>(result);

                    return pokemonModel;
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

        }

        public async Task<PokemonDto> GetPokemonWithTranslation(string name)
        {
            var pokemon = await GetPokemonInformation(name);
            if (pokemon == null)
                throw new Exception("Failed to get pokemon");

            var translatedString = "";
            // Maybe instead of hardcoding the translateLangauge, we could create an interface
            if (pokemon?.PokemonHabitat?.Name == "cave" || pokemon?.IsLegendary == true)
                translatedString = await translateService.TranslateText(pokemon.GetDescriptionByLangauge("en"), "yoda");
            else
                translatedString = await translateService.TranslateText(pokemon.GetDescriptionByLangauge("en"), "shakespeare");
            
            var pokemonDto = new PokemonDto
            {
                Name = pokemon?.Name,
                Habitat = pokemon?.PokemonHabitat?.Name,
                IsLegendary = pokemon.IsLegendary,
                Description = translatedString ?? pokemon?.GetDescriptionByLangauge("en")
            };

            return pokemonDto;
        }
    }
}
