using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public class TranslateService : ITranslateService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<TranslateService> logger;
        private readonly string baseUrl = "https://api.funtranslations.com/translate/";

        public TranslateService(IHttpClientFactory clientFactory, ILogger<TranslateService> logger)
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        public async Task<string> TranslateText(string text, string translateLanaguge)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + translateLanaguge);

            var postContent = new { text = text };

            var payload = new StringContent(JsonSerializer.Serialize(postContent), Encoding.UTF8, "application/json");
            request.Content = payload;

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                using (var result = await response.Content.ReadAsStreamAsync())
                {
                    var translateResult = await JsonSerializer.DeserializeAsync<TranslateModel>(result);

                    //  return pokemonModel;
                    return translateResult.Contents.TranslatedText;
                }
            }
            else
            {
                logger.LogError(response.ReasonPhrase);
                return null;
            }
        }
    }
}
