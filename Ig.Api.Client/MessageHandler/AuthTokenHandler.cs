using System.Net.Http.Headers;

namespace Ig.Api.Client.MessageHandler;

public class AuthTokenHandler(IAuthService authService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = authService.GetToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Access_token);

        var task = await base.SendAsync(request, cancellationToken);
        return task;
    }
}