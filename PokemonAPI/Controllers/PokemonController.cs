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

            if (pokemon == null)
                return BadRequest("Pokemon Not Found");

            var pokemonDto = new PokemonDto()
            {
                Name = pokemon.Name,
                Description = pokemon.GetDescriptionByLangauge("en"),
                Habitat = pokemon.PokemonHabitat.Name,
                IsLegendary = pokemon.IsLegendary
            };

            return Ok(pokemonDto);
        }

        [HttpGet("translated/{name}")]
        public async Task<ActionResult<PokemonDto>> GetPokemonWithTranslation(string name)
        {
           var pokemon = await pokemonService.GetPokemonWithTranslation(name);

           return Ok(pokemon);
        }

    }
}
