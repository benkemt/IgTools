using Ig.Api.Client.Model;
using System.Text.Json;
using Ig.Api.Client.Helpers;
using System.Net.Http.Headers;

namespace Ig.Api.Client;

public class IgLoginClient(HttpClient httpClient) : IIgLoginClient
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<Result<AccountResponse>> LoginAsync(AuthenticationRequest request)
    {
        using var memoryContentStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryContentStream, request, JsonSerializerOptionsWrapper.Options);
        memoryContentStream.Seek(0, SeekOrigin.Begin);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"gateway/deal/session");
        requestMessage.Headers.Add("Version", "3");

        using var streamContent = new StreamContent(memoryContentStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestMessage.Content = streamContent;

        try
        {
            using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            var stream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode)
            {

                var data = JsonSerializer.Deserialize<AccountResponse>(stream, JsonSerializerOptionsWrapper.Options);
                if( data is null)
                {
                    return Result<AccountResponse>.Failure("EmptyResponse");
                }
                else
                {
                    return Result<AccountResponse>.Success(data);
                }
            }
            else
            {
                var errorDetail = JsonSerializer.Deserialize<ErrorDto>(stream, JsonSerializerOptionsWrapper.Options);

                return Result<AccountResponse>.Failure(errorDetail != null ? errorDetail.ErrorCode : "Unknown");
            }
        }
        catch (HttpRequestException)
        {
            return Result<AccountResponse>.Failure("Connection.Error");
        }
    }
}