using ImageProcessor.Models;
using ImageProcessor.Models.LLMConfigs;
using ImageProcessor.Services.Converters.Interfaces;
using Newtonsoft.Json;

namespace ImageProcessor.Services.Converters;

public class JsonConverter : IJsonConverter
{
    private readonly JsonSerializerSettings _settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore //Ignore null values. 
    };
    public string NormalizeJson(string responseContent)
    {
        var content = JsonConvert.DeserializeObject<RawLlmResponse>(responseContent)?
            .Choices
            .FirstOrDefault()?
            .Message
            .Content
            .Trim();
        
        if (content is null) throw new InvalidOperationException($"Error deserializing response object: {responseContent}");
        var lines = content.Split('\n');
        content = string.Join('\n', lines[1..^1]);

        return content;
    }
    
    public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, _settings);

    public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings) ?? throw new InvalidOperationException($"Failed to deserialize to {typeof(T).Name}");
}