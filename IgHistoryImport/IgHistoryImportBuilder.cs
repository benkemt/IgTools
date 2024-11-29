using Ig.Api.Client;
using Ig.Api.Client.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;

namespace IgHistoryImport;

public class IgHistoryImportBuilder
{
    private IServiceProvider? _serviceProvider;
    private IConfiguration _configuration;

    private IgHistoryImportBuilder()
    {
        var services = new ServiceCollection();

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        IConfiguration configuration = configurationBuilder.Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddIgClient("https://demo-api.ig.com/", configuration["IgApiKey"] ?? "");

        _serviceProvider = services.BuildServiceProvider();
        _configuration = configuration;
    }

    public static IgHistoryImportBuilder Create()
    {
        return new IgHistoryImportBuilder();
    }

    public async Task ExecuteAsync( string[] args, 
        Func<ArgOption, IConfiguration , IIgClient, IAuthService,Task> actionParsed,  Action<IEnumerable<Error>> actionNotParsed)
    {
        if (_serviceProvider is null) throw new ArgumentNullException(nameof(_serviceProvider));

        var igClient = IgClientFactory.CreateIgClient(_serviceProvider);
        var authService = IgClientFactory.CreateAuthService(_serviceProvider);

        // Parse command-line arguments
        var res = await Parser.Default.ParseArguments<ArgOption>(args)
            .WithParsedAsync(async options =>
            {
                await actionParsed(options, _configuration, igClient, authService);
            });

        res.WithNotParsed(actionNotParsed);
    }

}