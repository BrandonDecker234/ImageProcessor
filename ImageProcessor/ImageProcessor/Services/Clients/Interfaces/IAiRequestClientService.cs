using ImageProcessor.Models;

namespace ImageProcessor.Services.Interfaces;

public interface IAiRequestClientService
{
    Task<ImageAnalysisResult> AnalyzeImageAsync(string imageUrl, string model);
}