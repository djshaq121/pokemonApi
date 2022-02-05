using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public interface ITranslateService
    {
        public Task<string> TranslateText(string text, string translateLanaguge);
    }
}