namespace ImageProcessor.Services.Converters.Interfaces;

public interface IImageConverter
{
    Task<string> ConvertImageToBase64(IFormFile file);
}