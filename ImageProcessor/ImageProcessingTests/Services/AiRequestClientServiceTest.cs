using System.Net;
using CodeVault.HttpClient;
using ImageProcessor.Models;
using ImageProcessor.Services.Builders.Interfaces;
using ImageProcessor.Services.Clients;
using ImageProcessor.Services.Clients.Interfaces;
using ImageProcessor.Services.Converters.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using HttpClient = CodeVault.HttpClient.HttpClient;

namespace ImageProcessingTests.Services;

public class AiRequestClientServiceTests
{
    private readonly Mock<IHttpClient> _httpMock = new();
    private readonly Mock<IAiPromptBuilder> _builderMock = new();
    private readonly Mock<IJsonConverter> _jsonMock = new();
    private readonly ILogger<AiRequestClientService> _logger = new Mock<ILogger<AiRequestClientService>>().Object;
    private readonly AiRequestClientService _service;

    public AiRequestClientServiceTests()
    {
        _service = new AiRequestClientService(
            _httpMock.Object,
            _builderMock.Object,
            _jsonMock.Object,
            _logger
        );
    }

    [Theory]
    [InlineData("", "model")]
    [InlineData("imageUrl", "")]    
    public async Task AnalyzeImageAsync_InvalidArguments_Throws(string imageUrl, string model)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeImageAsync(imageUrl, model));
    }

    [Fact]
    public async Task AnalyzeImageAsync_HttpError_ThrowsInvalidOperationException()
    {
        // Arrange
        const string imageUrl = "data:image/png;base64,AAA";
        const string model = "test-model";

        _httpMock
            .Setup(h => h.Post(It.IsAny<Request>()))
            .ReturnsAsync(new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccessStatusCode = false,
                Content = It.IsAny<string>()
            });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AnalyzeImageAsync(imageUrl, model));
    }

    [Fact]
    public async Task AnalyzeImageAsync_Success_ReturnsDeserializedResult()
    {
        // Arrange
        const string imageUrl = "data:image/png;base64,AAA";
        const string model = "test-model";
        _builderMock
            .Setup(b => b.BuildSystemPrompt(It.IsAny<LlmRequestConfigs>()))
            .Callback<LlmRequestConfigs>(_ => { });
        _builderMock
            .Setup(b => b.BuildUserPrompt(It.IsAny<LlmRequestConfigs>(), imageUrl))
            .Callback<LlmRequestConfigs, string>((_, _) => { });

        const string serialized = "{serialized}";
        _jsonMock.Setup(j => j.Serialize(It.IsAny<LlmRequestConfigs>())).Returns(serialized);

        const string rawJson = "```json { \"tags\":[{\"name\":\"cat\",\"confidence\":0.8}] }```";
        const string normalized = "{ \"tags\":[{\"name\":\"cat\",\"confidence\":0.8}] }";
        _httpMock
            .Setup(h => h.Post(It.IsAny<Request>()))
            .ReturnsAsync(new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = rawJson
            });

        _jsonMock
            .Setup(j => j.NormalizeJson(rawJson))
            .Returns(normalized);

        var expectedResult = new ImageAnalysisResult
        {
            Tags = { new ImageTag { Name = "cat", Confidence = 0.8 } }
        };
        _jsonMock
            .Setup(j => j.Deserialize<ImageAnalysisResult>(normalized))
            .Returns(expectedResult);

        // Act
        var result = await _service.AnalyzeImageAsync(imageUrl, model);

        // Assert
        Assert.Same(expectedResult, result);
        _builderMock.Verify(b => b.BuildSystemPrompt(It.IsAny<LlmRequestConfigs>()), Times.Once);
        _builderMock.Verify(b => b.BuildUserPrompt(It.IsAny<LlmRequestConfigs>(), imageUrl), Times.Once);
        _httpMock.Verify(h => h.Post(It.IsAny<Request>()), Times.Once);
        _jsonMock.Verify(j => j.NormalizeJson(rawJson), Times.Once);
        _jsonMock.Verify(j => j.Deserialize<ImageAnalysisResult>(normalized), Times.Once);
    }
}