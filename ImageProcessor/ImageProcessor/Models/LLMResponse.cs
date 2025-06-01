using System.Text.Json.Serialization;

namespace ImageProcessor.Models;

public class RawLlmResponse
{
    [JsonPropertyName("choices")]
    public required List<Choice> Choices { get; init; } = [];
}

public abstract class Choice
{
    [JsonPropertyName("message")]
    public LlmMessage Message { get; set; } = new();
}

public class LlmMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}