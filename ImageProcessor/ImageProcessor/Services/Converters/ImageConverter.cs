using ImageProcessor.Services.Converters.Interfaces;

namespace ImageProcessor.Services.Converters;

public class ImageConverter : IImageConverter
{
    public async Task<string> ConvertImageToBase64(IFormFile file)
    {
        await using var ms = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(ms);
        return $"data:{file.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
    }
}