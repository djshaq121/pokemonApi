using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonAPI;
using PokemonAPI.Controllers;
using PokemonAPI.DTOs;
using PokemonAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonApp.Tests
{
    public class PokemonControllerTests
    {
        private readonly Mock<IPokemonService> pokemonServiceMock = new Mock<IPokemonService>();
        private readonly PokemonController pokemonController;
        public PokemonControllerTests()
        {
            pokemonController = new PokemonController(pokemonServiceMock.Object);
        }

        [Fact]
        public async Task GetPokemon_Succesfully()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokemonModel = PokemonTestHelper.CreatePokemonModel(pokemonName, true, "cave");

            pokemonServiceMock.Setup(x => x.GetPokemonInformation(pokemonName))
                .ReturnsAsync(pokemonModel);

            // Act
            var actionResult = await pokemonController.GetPokemon(pokemonName);
            // Assert

            var result = (actionResult.Result as OkObjectResult).Value as PokemonDto;

            Assert.NotNull(result);
            Assert.Equal(pokemonName, result.Name);
        }

        [Fact]
        public async Task GetPokemon_Where_PokemoneService_returns_null()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var expectedError = "Pokemon Not Found";

            pokemonServiceMock.Setup(x => x.GetPokemonInformation(pokemonName))
                .ReturnsAsync((PokemonModel)null);

            // Act
            var actionResult = await pokemonController.GetPokemon(pokemonName);
            // Assert

            var result = (actionResult.Result as BadRequestObjectResult).Value as string;

            Assert.NotNull(result);
            Assert.Equal(expectedError, result);
        }

    }

}
