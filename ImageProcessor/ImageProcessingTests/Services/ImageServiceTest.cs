using ImageProcessor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageProcessingTests.Services;

public class ImageServiceTest
{
    private readonly ImageService _service = new(new NullLogger<ImageService>());

    private static MemoryStream CreateTestImage(int width, int height)
    {
        var image = new Image<Rgba32>(width, height);
        var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Position = 0;
        return ms;
    }
    
    [Fact]
    public async Task ApplyGrayscaleAsync_Stream_ProducesGrayImage()
    {
        using var input = CreateTestImage(10, 10);
        await using var output = await _service.ApplyGrayscaleAsync(input);

        output.Position = 0;
        using var img = await Image.LoadAsync<Rgba32>(output);
        var pixel = img[0, 0];
        Assert.Equal(pixel.R, pixel.G);
        Assert.Equal(pixel.G, pixel.B);
    }

    [Fact]
    public async Task ResizeAsync_Stream_ResizesImage()
    {
        using var input = CreateTestImage(8, 4);
        await using var output = await _service.ResizeAsync(input, 4, 2);

        output.Position = 0;
        using var img = await Image.LoadAsync<Rgba32>(output);
        Assert.Equal(4, img.Width);
        Assert.Equal(2, img.Height);
    }

    [Fact]
    public async Task RotateAsync_Stream_RotatesDimensions()
    {
        using var input = CreateTestImage(2, 3);
        await using var output = await _service.RotateAsync(input, 90);

        output.Position = 0;
        using var img = await Image.LoadAsync<Rgba32>(output);
        Assert.Equal(3, img.Width);
        Assert.Equal(2, img.Height);
    }

    [Fact]
    public async Task ApplyGrayscaleAsync_IFormFile_DelegatesToStreamOverload()
    {
        using var inputStream = CreateTestImage(5, 5);
        IFormFile file = new FormFile(inputStream, 0, inputStream.Length, "Data", "test.png");
        await using var output = await _service.ApplyGrayscaleAsync(file);

        output.Position = 0;
        using var img = await Image.LoadAsync<Rgba32>(output);
        var pixel = img[0, 0];
        Assert.Equal(pixel.R, pixel.G);
        Assert.Equal(pixel.G, pixel.B);
    }
}