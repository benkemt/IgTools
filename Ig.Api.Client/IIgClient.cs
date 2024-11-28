using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public interface IIgClient
{
    Task<Result<PriceList>> GetHistoricalPricesAsync( string epic, Resolution resolution, DateTime startDate, DateTime endDate );
}