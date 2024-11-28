using System.Net.Http.Headers;

namespace Ig.Api.Client.MessageHandler;

public class AuthTokenHandler(IAuthService authService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = authService.GetToken();
        var accountId = authService.GetAccountId();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Access_token);
        request.Headers.Add("IG-ACCOUNT-ID", accountId);

        var task = await base.SendAsync(request, cancellationToken);
        return task;
    }
}