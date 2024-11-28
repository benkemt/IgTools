namespace Ig.Api.Client;

public class IgClient : IIgClient
{
    private readonly HttpClient _httpClient;

    public IgClient( HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}