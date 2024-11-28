using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public interface IIgLoginClient
{
    Task<Result<AccountResponse>> LoginAsync( AuthenticationRequest request );
}