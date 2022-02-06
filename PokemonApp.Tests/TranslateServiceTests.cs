using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PokemonAPI;
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
    public class TranslateServiceTests
    {
        private readonly ITranslateService translateService;
        private readonly Mock<IHttpClientFactory> clientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<ILogger<TranslateService>> loggerMock = new Mock<ILogger<TranslateService>>();
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        public TranslateServiceTests()
        {
            translateService = new TranslateService(clientFactoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task TranslateText_When_StatusCode_Is_BadRequest()
        {
            // Arrange
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

            // Act
            var result = await translateService.TranslateText(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TranslateText_When_Text_Is_Translated_Successfully()
        {
            // Arrange
            var translatedModel = new TranslateModel()
            {
                Contents = new Contents()
                {
                    OriginalText = "text to translate",
                    TranslatedText = "translate text to",
                    Translation = "yoda"
                }
            };

            mockHttpMessageHandler.Protected()
              .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(translatedModel)),
              });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                   .Returns(client);

            // Act
            var result = await translateService.TranslateText(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Equal(translatedModel.Contents.TranslatedText, result);
        }
    }
}
