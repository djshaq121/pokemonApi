using Microsoft.AspNetCore.Mvc;
using PokemonAPI.DTOs;
using PokemonAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            this.pokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PokemonDto>> GetPokemon(string name)
        {
            var pokemon = await pokemonService.GetPokemonInformation(name);
            var pokemonDto = new PokemonDto()
            {
                Name = pokemon.Name,
                Description = pokemon.GetDescriptionByLangauge("en"),
                Habitat = pokemon.Habitat.Name,
                IsLegendary = pokemon.IsLegendary
            };

            return Ok(pokemonDto);
        }

    }
}
