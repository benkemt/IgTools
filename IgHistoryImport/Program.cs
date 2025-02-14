using com.lightstreamer.client;
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
    IAuthService authService,
    IIgStreamClient streamClient)
{
    var res = await authService.LoginAsync(configuration["IgUserName"] ?? "", configuration["IgPassword"] ?? "");

    if (!res.IsSuccess)
    {
        Console.WriteLine($"Login Failed : '{res.ErrorCode}'");
        return;
    }

    Console.WriteLine($"Login to igApi");


    var clientListener = new StreamClientListener();

    streamClient.LoginAsync(clientListener);

    Console.ReadLine();

    return;

    TimeSpan span = argOption.EndDate - argOption.StartDate;
    double dayCount = span.TotalDays +1;

    for(int i = 0; i < dayCount; i++)
    {
        var currentDate = argOption.StartDate.AddDays(i);

        if (currentDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            Console.WriteLine($"Skip Date : {currentDate}");
            continue;
        }

        var startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 08, 0, 0);
        var endDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 22, 0, 0);

        var historicalPrices = await igClient.GetHistoricalPricesAsync(argOption.Epic, Resolution.MINUTE, startDate, endDate);

        if (historicalPrices.IsSuccess)
        {
            if (historicalPrices.Data is null)
            {
                Console.WriteLine($"No data for {argOption.Epic} from {startDate} to {endDate}");
                return;
            }

            Console.WriteLine($"Historical Prices for {argOption.Epic} from {startDate} to {endDate} : {historicalPrices.Data.Prices.Count} Items");

            // Serialize the historical prices to JSON
            var json = JsonSerializer.Serialize(historicalPrices.Data.Prices, new JsonSerializerOptions { WriteIndented = true });
            // Write the JSON data to a file
            var fileName = $"{startDate:yyyy-MM-dd}.json";
            //create a directory if it does not exist
            Directory.CreateDirectory("data");
            await File.WriteAllTextAsync($"data\\{fileName}", json);

            Console.WriteLine($"Historical prices saved to {fileName}");

            Console.WriteLine($"Allowance : '{historicalPrices.Data.MetaData.Allowance.RemainingAllowance}'");
        }
        else
        {
            Console.WriteLine($"Failed to get Historical Prices for {argOption.Epic} : '{historicalPrices.ErrorCode}'");
        }
    }
}


static void Errors(IEnumerable<CommandLine.Error> errors)
{
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}

//.\IgHistoryImport.exe --epic IX.D.CAC.IDF.IP --startDate 2024-11-25 --endDate 2024-11-27