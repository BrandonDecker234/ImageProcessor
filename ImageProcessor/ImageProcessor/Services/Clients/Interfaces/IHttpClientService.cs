using RestSharp;

namespace ImageProcessor.Services.Clients.Interfaces;

public interface  IHttpClientService
{
    Task<RestResponse> Get(object obj, RestRequest request);
    Task<RestResponse> Get(RestRequest request);
    Task<RestResponse> Post(object obj, RestRequest request);
    Task<RestResponse> Post(string jsonBody, RestRequest request);
}