using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public class AuthService( IIgLoginClient igLoginClient) : IAuthService
{
    private readonly IIgLoginClient _igLoginClient = igLoginClient ?? throw new ArgumentNullException(nameof(igLoginClient));

    private OAuthToken? _authToken = null;
    private string? _accountId = null;
    private string? _lightstreamerEndpoint = null;
    private string? _cst = null;
    private string? _securityToken = null;

    public async Task<Result> LoginAsync(string username, string password)
    {
        var request = new AuthenticationRequest
        {
            Identifier = username,
            Password = password
        };

        var result = await GetTokenAsync(request);

        if (result.IsSuccess)
        {
            return Result.Success();
        }
        else
        {
            _authToken = null;
            _accountId = null;
            _lightstreamerEndpoint = null;
            return Result.Failure(result.ErrorCode);
        }
    }

    public string? GetAccountId()
    {
        return _accountId;
    }

    public OAuthToken? GetToken()
    {
       return _authToken;
    }

    public string? GetCstToken()
    {
        return _cst;
    }

    public string? GetSecurityToken()
    {
        return _securityToken;
    }

    public string? GetLightStreamServerAddress()
    {
        return _lightstreamerEndpoint;
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
                _lightstreamerEndpoint = response.Data?.LightstreamerEndpoint;

                if( _authToken is null || _accountId is null || _lightstreamerEndpoint is null)
                {
                    return Result.Failure("EmptyToken");
                }

                var sessionDetailResponse = await _igLoginClient.GetSessionDetailAsync(true, _authToken.Access_token, _accountId);

                if( sessionDetailResponse.IsFailure)
                {
                    return Result.Failure(sessionDetailResponse.ErrorCode);
                }
                else
                {
                    _cst = sessionDetailResponse.Data?.Cst;
                    _securityToken = sessionDetailResponse.Data?.SecurityToken;
                }
              


                return Result.Success();
            }
        }
    }
}