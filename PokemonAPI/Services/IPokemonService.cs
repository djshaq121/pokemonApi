using PokemonAPI.DTOs;
using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public interface IPokemonService
    {
        public Task<PokemonModel> GetPokemonInformation(string pokemonName);
    }
}