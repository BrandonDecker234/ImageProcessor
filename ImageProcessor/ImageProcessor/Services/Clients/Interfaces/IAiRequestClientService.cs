using ImageProcessor.Models;

namespace ImageProcessor.Services.Clients.Interfaces;

public interface IAiRequestClientService
{
    Task<ImageAnalysisResult> AnalyzeImageAsync(string imageUrl, string model);
}