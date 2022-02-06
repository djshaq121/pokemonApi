using Moq;
using Moq.Protected;
using PokemonAPI;
using PokemonAPI.DTOs;
using PokemonAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PokemonApp.Tests
{
    public class PokemonServiceTests
    {
        private readonly IPokemonService pokemonService;
        private readonly Mock<IHttpClientFactory> clientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<ITranslateService> translateServiceMock = new Mock<ITranslateService>();
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        public PokemonServiceTests()
        {
            pokemonService = new PokemonService(clientFactoryMock.Object, translateServiceMock.Object);
        }

        [Fact]
        public async Task GetPokemonInformation_Where_Api_Returns_BadRequest()
        {
            var pokemon = "mewtwo";
            
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(""),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                   .Returns(client);

            await Assert.ThrowsAsync<Exception>(() => pokemonService.GetPokemonInformation(pokemon));
        }

        [Fact]
        public async Task GetPokemonInformation_Where_Api_Gets_Pokemon()
        {
            // Arrange
            var pokemon = new PokemonModel
            {
                Name = "wormadam",
                IsLegendary = true
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"name\":\"wormadam\",\"is_legendary\":true,\"habitat\":{\"name\":\"cave\"}}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                   .Returns(client);

            // Act 
            var result = await pokemonService.GetPokemonInformation(pokemon.Name);

            // Assert
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
            Assert.Equal(pokemon.Name, result.Name);
        }

        [Fact]
        public async Task GetPokemonWithTranslation_Succeeds()
        {
            // Arrange
            var pokemon = new PokemonDto
            {
                Name = "wormadam",
                IsLegendary = true,
                Description = ""
            };

            var expectedPokemonDescription = "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was.";

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"name\":\"wormadam\",\"is_legendary\":true,\"habitat\":{\"name\":\"cave\"}}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                   .Returns(client);

            translateServiceMock.Setup(x => x.TranslateText(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedPokemonDescription);

            // Act 
            var result = await pokemonService.GetPokemonWithTranslation(pokemon.Name);

            // Assert
            Assert.Equal(expectedPokemonDescription, result.Description);
        }

        [Fact]
        public async Task GetPokemonWithTranslation_Translation_Returns_null()
        {
            // Arrange
            var pokemon = new PokemonDto
            {
                Name = "wormadam",
                IsLegendary = true,
                Description = ""
            };

            var pokemonModel = PokemonTestHelper.CreatePokemonModel(pokemon.Name, pokemon.IsLegendary, "cave");

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(pokemonModel)),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                   .Returns(client);

            translateServiceMock.Setup(x => x.TranslateText(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string)null);

            // Act 
            var result = await pokemonService.GetPokemonWithTranslation(pokemon.Name);

            // Assert
            Assert.Equal(pokemonModel.GetDescriptionByLangauge("en"), result.Description);
        }

    }
}
