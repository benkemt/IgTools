using Ig.Api.Client;

using Microsoft.Extensions.DependencyInjection;
using Ig.Api.Client.Extension;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

IConfiguration configuration = configurationBuilder.Build();
services.AddSingleton<IConfiguration>(configuration);


services.AddIgClient("https://demo-api.ig.com/", configuration["IgApiKey"] ?? "");




var serviceProvider = services.BuildServiceProvider();

var igClient = IgClientFactory.CreateIgClient(serviceProvider);
var authService  =IgClientFactory.CreateAuthService(serviceProvider);



var config = serviceProvider.GetRequiredService<IConfiguration>();



var res = await authService.LoginAsync(config["IgUserName"] ?? "", config["IgPassword"] ?? "");

if(res.IsSuccess)
{
    Console.WriteLine("Login Success");
}
else
{
    Console.WriteLine($"Login Failed : '{res.ErrorCode}'" );
}


