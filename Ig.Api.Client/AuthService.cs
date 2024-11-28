using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public class AuthService( IIgLoginClient igLoginClient) : IAuthService
{
    private readonly IIgLoginClient _igLoginClient = igLoginClient ?? throw new ArgumentNullException(nameof(igLoginClient));

    private OAuthToken? _authToken = null;
    private string? _accountId = null;


    public async Task<Result> LoginAsync(string username, string password)
    {
        var request = new AuthenticationRequest
        {
            Identifier = username,
            Password = password
        };

        var login = await GetTokenAsync(request);

        if (login.IsSuccess)
        {
            return Result.Success();
        }
        else
        {
            _authToken = null;
            _accountId = null;
            return Result.Failure(login.ErrorCode);
        }
    }

    public OAuthToken? GetToken()
    {
       return _authToken;
    }

    public string? GetAccountId()
    {
        return _accountId;
    }

    private  async Task<Result> GetTokenAsync(AuthenticationRequest request)
    {
        var response = await _igLoginClient.LoginAsync(request);

        if (response.IsFailure)
        {
            return Result.Failure(response.ErrorCode);
        }
        else
        {
            if (response.Data?.OauthToken is null)
            {
                return Result.Failure("EmptyToken");
            }
            else
            {
                _authToken = response.Data?.OauthToken;
                _accountId = response.Data?.AccountId;
                return Result.Success();
            }
        }
    }
}