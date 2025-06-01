using ImageProcessor.Controllers;
using ImageProcessor.Models;
using ImageProcessor.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImageProcessingTests.Controllers;

public class ImageAnalysisControllerTest
{
    private readonly Mock<IImageAnalysisService> _mockAnalysisService;
    private readonly ImageAnalysisController _controller;

    public ImageAnalysisControllerTest()
    {
        _mockAnalysisService = new Mock<IImageAnalysisService>();
        var mockMetaDataService = new Mock<IMetaDataService>();
        var mockLogger = new Mock<ILogger<ImageAnalysisController>>();
        _controller = new ImageAnalysisController(_mockAnalysisService.Object, mockLogger.Object, mockMetaDataService.Object);
    }

    [Fact]
    public async Task GetImageAnalysis_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("test.jpg");
        mockFile.Setup(f => f.Length).Returns(1024);

        var expectedResult = new ImageAnalysisResult { Tags = new  List<ImageTag>([
                new ImageTag { Name = "tag1", Confidence = 0.9 },
                new ImageTag { Name = "tag2", Confidence = 0.8  }
            ])
        };
        _mockAnalysisService.Setup(s => s.AnalyzeImage(mockFile.Object, "model"))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetImageAnalysis(mockFile.Object, "model");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<ImageAnalysisResult>(okResult.Value);
        Assert.Equal(expectedResult.Tags[0], actualResult.Tags[0]);
    }

    [Fact]
    public async Task GetImageAnalysis_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("test.jpg");
        mockFile.Setup(f => f.Length).Returns(1024);

        _mockAnalysisService.Setup(s => s.AnalyzeImage(mockFile.Object, "model1"))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetImageAnalysis(mockFile.Object, "model1");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

    }
}