using ImageProcessor.Models;

namespace ImageProcessor.Services.Interfaces;

public interface IImageAnalysisService
{
    Task<ImageAnalysisResult> AnalyzeImage(IFormFile file, string model = "gemma3:12b");
}