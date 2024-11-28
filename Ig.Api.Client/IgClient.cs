using Ig.Api.Client.Helpers;
using Ig.Api.Client.Model;
using System.Text.Json;

namespace Ig.Api.Client;

public class IgClient(HttpClient httpClient) : IIgClient
{
    public async Task<Result<PriceList>> GetHistoricalPricesAsync(string epic, Resolution resolution, DateTime startDate, DateTime endDate)
    {
        var startDateString = startDate.ToString("yyyy-MM-dd HH:mm:ss");
        var enDateString = endDate.ToString("yyyy-MM-dd HH:mm:ss");
        var resolutionString = Enum.GetName(resolution);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"gateway/deal/prices/{epic}/{resolutionString}/{startDateString}/{enDateString}");
        requestMessage.Headers.Add("Version", "2");

        try
        {
            using var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            var se = await response.Content.ReadAsStringAsync();

            var stream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode)
            {

                var data = JsonSerializer.Deserialize<PriceList>(stream, JsonSerializerOptionsWrapper.Options);
                if (data is null)
                {
                    return Result<PriceList>.Failure("EmptyResponse");
                }
                else
                {
                    return Result<PriceList>.Success(data);
                }
            }
            else
            {
                var errorDetail = JsonSerializer.Deserialize<ErrorDto>(stream, JsonSerializerOptionsWrapper.Options);

                return Result<PriceList>.Failure(errorDetail != null ? errorDetail.ErrorCode : "Unknown");
            }
        }
        catch (HttpRequestException)
        {
            return Result<PriceList>.Failure("Connection.Error");
        }

    }
}