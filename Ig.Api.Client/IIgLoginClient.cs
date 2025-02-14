using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public interface IIgLoginClient
{
    Task<Result<AccountResponse>> LoginAsync( AuthenticationRequest request );

    Task<Result<SessionDetailResponse>> GetSessionDetailAsync(bool fetchSessionTokens, string accessToken, string accountId);
}