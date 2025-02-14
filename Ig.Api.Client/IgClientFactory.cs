using Microsoft.Extensions.DependencyInjection;

namespace Ig.Api.Client;

public static class IgClientFactory
{
    public static IIgClient CreateIgClient(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
        return serviceProvider.GetRequiredService<IIgClient>();
    }

    public static IAuthService CreateAuthService(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
        return serviceProvider.GetRequiredService<IAuthService>();
    }

    public static IIgStreamClient CreateIgStreamClient(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
        return serviceProvider.GetRequiredService<IIgStreamClient>();
    }
}