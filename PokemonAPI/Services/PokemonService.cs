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
        public PokemonService(IHttpClientFactory httpClient)
        {
            clientFactory = httpClient;
        }

        public async Task<PokemonModel> GetPokemonInformation(string pokemonName)
        {
            var uri = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonName;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
           
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
    }
}
