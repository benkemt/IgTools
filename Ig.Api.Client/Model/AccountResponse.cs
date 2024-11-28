
namespace Ig.Api.Client.Model;

public class AccountResponse
{
    public string AccountId { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string LightstreamerEndpoint { get; set; } = null!;
    public OAuthToken OauthToken { get; set; } = null!;
    public int TimezoneOffset { get; set; }
}

public class OAuthToken
{
    public string Access_token { get; set; } = null!;
    public string Expires_in { get; set; } = null!;
    public string Refresh_token { get; set; } = null!;
    public string Scope { get; set; } = null!;
    public string Token_type { get; set; } = null!;

}