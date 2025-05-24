namespace ImageProcessor.Services;

public interface IImageService
{
    Task<Stream> ApplyGrayscaleAsync(IFormFile file);
    Task<Stream> ResizeAsync(IFormFile file, int width, int height);
    Task<Stream> RotateAsync(IFormFile file, int degrees);
}