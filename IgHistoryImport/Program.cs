using Ig.Api.Client;
using Ig.Api.Client.Model;
using IgHistoryImport;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

await IgHistoryImportBuilder.Create()
    .ExecuteAsync(args, GetHistoricPrice, Errors);
return 0;

static async Task GetHistoricPrice(
    ArgOption argOption, 
    IConfiguration configuration, 
    IIgClient igClient, 
    IAuthService authService)
{
    var res = await authService.LoginAsync(configuration["IgUserName"] ?? "", configuration["IgPassword"] ?? "");

    if (!res.IsSuccess)
    {
        Console.WriteLine($"Login Failed : '{res.ErrorCode}'");
        return;
    }

    Console.WriteLine($"Login to igApi");

    var historicalPrices = await igClient.GetHistoricalPricesAsync(argOption.Epic, Resolution.MINUTE, argOption.StartDate, argOption.EndDate);

    if(historicalPrices.IsSuccess)
    {
        if (historicalPrices.Data is null)
        {
            return;
        }

        Console.WriteLine($"Historical Prices for {argOption.Epic}");

        // Serialize the historical prices to JSON
        var json = JsonSerializer.Serialize(historicalPrices.Data.Prices, new JsonSerializerOptions { WriteIndented = true });


        // Write the JSON data to a file
        var fileName = $"{argOption.StartDate:yyyy-MM-dd}.json";
        await File.WriteAllTextAsync(fileName, json);

        Console.WriteLine($"Historical prices saved to {fileName}");

        Console.WriteLine($"Allowance : '{historicalPrices.Data.MetaData.Allowance.RemainingAllowance}'");
    }
    else
    {
        Console.WriteLine($"Failed to get Historical Prices for {argOption.Epic} : '{historicalPrices.ErrorCode}'");
    }
}


static void Errors(IEnumerable<CommandLine.Error> errors)
{
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}