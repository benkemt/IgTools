
using System.Net.Http.Headers;
using Ig.Api.Client.MessageHandler;
using Microsoft.Extensions.DependencyInjection;

namespace Ig.Api.Client.Extension;

public static class IgClientExtension
{
    public static void AddIgClient(this IServiceCollection serviceCollection, string url, string apiKeyValue)
    {
        serviceCollection.AddSingleton<IAuthService, AuthService>();
        serviceCollection.AddTransient<AuthTokenHandler>();

        serviceCollection.AddHttpClient<IIgClient, IgClient> ((client) =>
        {
            client.DefaultRequestHeaders.Add("X-IG-API-KEY", apiKeyValue);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(url);
        }).AddHttpMessageHandler<AuthTokenHandler>();

        serviceCollection.AddHttpClient<IIgLoginClient, IgLoginClient>((client) =>
        {
            client.DefaultRequestHeaders.Add("X-IG-API-KEY", apiKeyValue);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(url);
        });

  

    }
}