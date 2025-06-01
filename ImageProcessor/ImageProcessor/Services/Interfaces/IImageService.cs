namespace ImageProcessor.Services.Interfaces;

public interface IImageService
{
    Task<Stream> ApplyGrayscaleAsync(IFormFile file);
    Task<Stream> ResizeAsync(IFormFile file, int width, int height);
    Task<Stream> RotateAsync(IFormFile file, int degrees);
    string SetDownloadFileName(string fileName);
}