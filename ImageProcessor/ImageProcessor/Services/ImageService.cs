using ImageProcessor.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessor.Services;

public class ImageService (ILogger<ImageService> logger) : IImageService
{
    private async Task<Stream> ProcessImageAsync(Stream input, Action<IImageProcessingContext> mutateAction)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        input.Position = 0;

        using var image = await Image.LoadAsync(input);
        logger.LogInformation($"Image loaded. Dimensions: {image.Width}x{image.Height}");
        image.Mutate(mutateAction);
        logger.LogInformation($"Image mutation applied: {mutateAction}");

        return await SaveImageAsAsync(image);
    }

    private async Task<MemoryStream> SaveImageAsAsync(Image image)
    {
        var format = image.Metadata.DecodedImageFormat ?? PngFormat.Instance; // Get the original image format. Default to PNG if we can't determine the extension.
        var encoder = image.Configuration.ImageFormatsManager.GetEncoder(format);
        
        var output = new MemoryStream();
        await image.SaveAsync(output, encoder); // Saves the image using the original format. Or fallback value
        logger.LogInformation($"Image saved to output stream as PNG. Output length={output.Length}");
        output.Position = 0;
        return output;
    }
    
    public async Task<Stream> ApplyGrayscaleAsync(Stream input)
    {
        return await ProcessImageAsync(input, action => action.Grayscale());
    }
    
    public Task<Stream> ApplyGrayscaleAsync(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        logger.LogInformation($"Applying Gray Scale: FileName={file.FileName}, Length={file.Length}");
        using var stream = file.OpenReadStream();
        return ApplyGrayscaleAsync(stream);
    }
    
    public async Task<Stream> ResizeAsync(Stream input, int width, int height)
    {
        return await ProcessImageAsync(input, action => action.Resize(width, height));
    }
    
    public Task<Stream> ResizeAsync(IFormFile file, int width, int height)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        return ResizeAsync(file.OpenReadStream(), width, height);
    }
    
    public async Task<Stream> RotateAsync(Stream input, int degrees)
    {
        return await ProcessImageAsync(input, action => action.Rotate(degrees));
    }
    
    public Task<Stream> RotateAsync(IFormFile file, int degrees)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        return RotateAsync(file.OpenReadStream(), degrees);
    }

    public string SetDownloadFileName(string fileName)
    {
        return $"{fileName}_{Guid.NewGuid()}";
    }
}