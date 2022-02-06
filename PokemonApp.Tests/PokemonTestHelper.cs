using PokemonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApp.Tests
{
    public static class PokemonTestHelper
    {
        public static PokemonModel CreatePokemonModel(string pokemonName, bool isLegendary, string habitat)
        {
            var pokemonModel = new PokemonModel();
            pokemonModel.Name = pokemonName;
            pokemonModel.IsLegendary = isLegendary;
            pokemonModel.FlavorTexts = new List<FlavorTextEntries>()
            {
                new FlavorTextEntries()
                {
                    Description = "Test description",
                    Language = new Language() { Name = "en" }
                }
            };
            pokemonModel.PokemonHabitat = new Habitat()
            {
                Name = habitat,
            };

            return pokemonModel;
        }
    }
}
