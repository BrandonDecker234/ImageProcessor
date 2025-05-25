using ImageProcessor.Models;
using ImageProcessor.Services.Builders.Interfaces;
using ImageProcessor.Services.Converters.Interfaces;
using ImageProcessor.Services.Interfaces;
using RestSharp;

namespace ImageProcessor.Services.Clients;

public class AiRequestClientService (
    IHttpClientService httpClientService,
    IAiPromptBuilder aiPromptBuilder,
    IJsonConverter jsonConverter,
    ILogger<AiRequestClientService> logger
    ) : IAiRequestClientService
{
    public async Task<ImageAnalysisResult> AnalyzeImageAsync(string imageUrl, string model)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model, nameof(model));
        ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl, nameof(imageUrl));
        
        var requestBody = ConfigureRequestBody(model, imageUrl);

        var request = new RestRequest("/chat/completions").AddHeader("Content-Type", "application/json");
        
        logger.LogDebug($"Starting request to LLM provider. Request body: {requestBody} {request}");
        
        var response = await httpClientService.Post(jsonConverter.Serialize(requestBody), request);
        
        logger.LogInformation($"Response was: {response}");
        
        if(!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content)) throw new InvalidOperationException($"Error while processing image. Response: {response.StatusCode} {response.ErrorMessage}");

        var content = jsonConverter.NormalizeJson(response.Content);
        
        var result = jsonConverter.Deserialize<ImageAnalysisResult>(content ?? throw new NullReferenceException("Error deserializing the response body"));

        return result ?? throw new NullReferenceException("Result object is null");

    }

    private LlmRequestConfigs ConfigureRequestBody(string model, string imageUrl)
    {
        var cfg = new LlmRequestConfigs { Model = model };
        aiPromptBuilder.BuildSystemPrompt(cfg);
        aiPromptBuilder.BuildUserPrompt(cfg, imageUrl);
        return cfg;
    }
}