using ImageProcessor.Models;
using ImageProcessor.Services.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace ImageProcessor.Services.Clients;

public class HttpClientService : IHttpClientService
{
    private readonly RestClient _client;

    public HttpClientService(IOptions<AuthenticationConfigs> configs)
    {
        if (configs.Value.Domain == null || configs.Value.ApiKey == null)
        {
            throw new ArgumentNullException(nameof(configs), "Authentication configs are required.");
        }

        var options = new RestClientOptions(configs.Value.Domain)
        {
            Authenticator = new JwtAuthenticator(configs.Value.ApiKey)
        };

        _client = new RestClient(options);
    }

    public async Task<RestResponse> Get(object obj, RestRequest request) => await _client.ExecuteGetAsync(request.AddObject(obj));

    public async Task<RestResponse> Get(RestRequest request) => await _client.ExecuteGetAsync(request);

    public async Task<RestResponse> Post(object obj, RestRequest request) => await _client.ExecutePostAsync(request.AddObject(obj));

    public async Task<RestResponse> Post(string obj, RestRequest request) => await _client.ExecutePostAsync(request.AddJsonBody(obj));
}