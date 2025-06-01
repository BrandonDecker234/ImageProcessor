using System.Text.Json.Serialization;

namespace ImageProcessor.Models;


public class ImageAnalysisResult
{
    [JsonPropertyName("tags")]
    public List<ImageTag> Tags { get; init; } = [];
}

public class ImageTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}