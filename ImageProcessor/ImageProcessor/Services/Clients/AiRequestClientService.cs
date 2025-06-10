using CodeVault.HttpClient;
using ImageProcessor.Models;
using ImageProcessor.Services.Builders.Interfaces;
using ImageProcessor.Services.Clients.Interfaces;
using ImageProcessor.Services.Converters.Interfaces;
using RestSharp;
using HttpClient = CodeVault.HttpClient.HttpClient;


namespace ImageProcessor.Services.Clients;

public class AiRequestClientService (
    HttpClient httpClient,
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
        var request = new Request("/chat/completions").AddBody(requestBody);
        logger.LogDebug($"Starting request to LLM provider. Request body: {requestBody} {request}");
        try
        {
            var response = await httpClient.Post(request);

        
            logger.LogInformation($"Response was: {response}");
        
            if(!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content)) throw new InvalidOperationException($"Error while processing image. Response: {response.StatusCode} {response.ErrorMessage}");

            var x = jsonConverter.NormalizeJson(response.Content);
            var result = jsonConverter.Deserialize<ImageAnalysisResult>(x);

            return result ?? throw new NullReferenceException("Result object is null");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        

    }

    private LlmRequestConfigs ConfigureRequestBody(string model, string imageUrl)
    {
        var cfg = new LlmRequestConfigs { Model = model };
        aiPromptBuilder.BuildSystemPrompt(cfg);
        aiPromptBuilder.BuildUserPrompt(cfg, imageUrl);
        return cfg;
    }
}