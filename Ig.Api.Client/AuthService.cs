using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public class AuthService( IIgLoginClient igLoginClient) : IAuthService
{
    private readonly IIgLoginClient _igLoginClient = igLoginClient ?? throw new ArgumentNullException(nameof(igLoginClient));

    private OAuthToken? _authToken = null;
  

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
            _authToken = login.Data;
            return Result.Success();
        }
        else
        {
            _authToken = null;
            return Result.Failure(login.ErrorCode);
        }
    }

    public OAuthToken? GetToken()
    {
       return _authToken;
    }

    private  async Task<Result<OAuthToken>> GetTokenAsync(AuthenticationRequest request)
    {
        var response = await _igLoginClient.LoginAsync(request);

        if (response.IsFailure)
        {
            return Result<OAuthToken>.Failure(response.ErrorCode);
        }
        else
        {
            var token = response.Data?.OauthToken;

            if (token == null)
            {
                return Result<OAuthToken>.Failure("EmptyToken");
            }
            else
            {
                return Result<OAuthToken>.Success(token);
            }
        }
    }
}