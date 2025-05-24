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
        logger.LogInformation("Image loaded. Dimensions: {Width}x{Height}", image.Width, image.Height);
        image.Mutate(mutateAction);
        logger.LogInformation("Image mutation applied: {mutateAction}", mutateAction);

        return await SaveImageAsAsync(image);
    }

    private async Task<MemoryStream> SaveImageAsAsync(Image image)
    {
        var output = new MemoryStream();
        var format = image.Metadata.DecodedImageFormat ?? PngFormat.Instance; // Get the original image format. Default to PNG if we can't determine the extension.
        var encoder = image.Configuration.ImageFormatsManager.GetEncoder(format); // Get the encoder for image format
        
        await image.SaveAsync(output, encoder); // Saves the image using the original format. (Unless it uses the fallback value)
        logger.LogInformation("Image saved to output stream as PNG. Output length={Length}", output.Length);
        output.Position = 0;
        return output;
    }
    
    public async Task<Stream> ApplyGrayscaleAsync(Stream input)
    {
        return await ProcessImageAsync(input, action => action.Grayscale());
    }
    
    public Task<Stream> ApplyGrayscaleAsync(IFormFile file)
    {
        logger.LogInformation("Applying Gray Scale: FileName={FileName}, Length={Length}", file.FileName, file.Length);
        ArgumentNullException.ThrowIfNull(file, nameof(file));
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
}