using ImageProcessor.Controllers;
using ImageProcessor.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImageProcessingTests.Controllers;

public class ImageControllerTest
{
    private readonly Mock<IImageService> _mockImageService;
    private readonly ImageController _controller;

    public ImageControllerTest()
    {
        _mockImageService = new Mock<IImageService>();
        var mockLogger = new Mock<ILogger<ImageController>>();
        _controller = new ImageController(_mockImageService.Object, mockLogger.Object);
    }
    
    private static Mock<IFormFile> CreateMockFormFile(string fileName = "test.jpg", string contentType = "image/jpeg", long length = 1024)
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.ContentType).Returns(contentType);
        mockFile.Setup(f => f.Length).Returns(length);
        // mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[length]));
        return mockFile;
    }
    private static MemoryStream CreateDummyStream(int size = 100)
    {
        var stream = new MemoryStream(new byte[size]);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
    
    #region RotateImage Tests

    [Fact]
    public async Task RotateImage_ValidRequest_ReturnsFileResult()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var expectedStream = CreateDummyStream();

        const int degrees = 90;
        const string expectedFileName = "rotated_image_timestamp.jpeg";

        _mockImageService.Setup(s => s.RotateAsync(mockFile.Object, degrees))
            .ReturnsAsync(expectedStream);
        _mockImageService.Setup(s => s.SetDownloadFileName(It.IsAny<string>()))
            .Returns(expectedFileName);

        // Act
        var result = await _controller.RotateImage(mockFile.Object, degrees);

        // Assert
        var fileResult = Assert.IsType<FileStreamResult>(result);
        Assert.Equal(expectedStream, fileResult.FileStream);
        Assert.Equal(mockFile.Object.ContentType, fileResult.ContentType);
        Assert.Equal(expectedFileName, fileResult.FileDownloadName);
    }

    [Fact]
    public async Task RotateImage_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        const int degrees = 90;
        const string exceptionMessage = "Rotation failed.";

        _mockImageService.Setup(s => s.RotateAsync(mockFile.Object, degrees))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.RotateImage(mockFile.Object, degrees);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        Assert.Equal("Image analysis failed.", statusCodeResult.Value);
    }

    [Fact]
    public async Task RotateImage_NullFile_ReturnsBadRequest()
    {
        // Arrange
        IFormFile? nullFile = null;
        const int degrees = 90;

        // Act
        var result = await _controller.RotateImage(nullFile!, degrees);

        // Assert
        var badRequestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal("Image analysis failed.", badRequestResult.Value);
    }

    [Fact]
    public async Task RotateImage_EmptyFile_ReturnsBadRequest()
    {
        // Arrange
        var emptyFile = CreateMockFormFile(length: 0).Object;
        const int degrees = 90;

        // Act
        var result = await _controller.RotateImage(emptyFile, degrees);

        // Assert
        var badRequestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal("Image analysis failed.", badRequestResult.Value);
    }

    #endregion
}