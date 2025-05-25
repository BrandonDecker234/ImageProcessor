using Newtonsoft.Json;

namespace ImageProcessor.Models;

public class LlmRequestConfigs
{
    [JsonProperty("model")]
    public required string Model { get; set; } //Name of the AI model to use for generating responses.

    [JsonProperty("messages")]
    public List<ChatMessage> Messages { get; set; } = []; //List of chat messages that will be sent to the AI model.
}

public class ChatMessage
{
    [JsonProperty("role")]
    public required string Role { get; set; } //Role of the chat message. Currently, "user" or "system".

    [JsonProperty("content")]
    public required List<Content> Content { get; set; } //Content of the chat message. This is the text that the AI model will use to generate responses.
}

public class ImageUrl
{
    [JsonProperty("url")]
    public string Url { get; set; }
}

public class Content
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("text")]
    public string Text { get; set; }
    
    [JsonProperty("image_url")]
    public ImageUrl ImageUrl { get; set; }
}