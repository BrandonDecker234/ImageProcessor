using ImageProcessor.Models;
using ImageProcessor.Services.Converters.Interfaces;
using ImageProcessor.Services.Interfaces;

namespace ImageProcessor.Services;

public class ImageAnalysisService (
    ILogger<ImageAnalysisService> logger, 
    IImageConverter imageConverter,
    IAiRequestClientService aiRequestClientService
    ): IImageAnalysisService
{
    public async Task<ImageAnalysisResult> AnalyzeImage(IFormFile file, string model = "gemma3:12b")
    {
        var urlString = await imageConverter.ConvertImageToBase64(file);
        logger.LogInformation($"Converted image to base64: {urlString}");
        var analysis = await aiRequestClientService.AnalyzeImageAsync(urlString, model);
        
        return analysis;
    }
}