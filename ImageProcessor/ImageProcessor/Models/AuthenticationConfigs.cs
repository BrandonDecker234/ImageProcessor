namespace ImageProcessor.Models;

public class AuthenticationConfigs(string apiKey, string domain)
{
    public string? ApiKey { get; set; } = apiKey;
    public string? Domain { get; set; } = domain;
}