namespace ImageProcessor.Models.Auth;

public class AuthenticationConfigs(string apiKey, string domain)
{
    public string ApiKey { get; } = apiKey;
    public string Domain { get; } = domain;
}