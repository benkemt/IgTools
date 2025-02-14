namespace Ig.Api.Client.Model;

public class SessionDetailResponse
{
    public string AccountId { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public string LightstreamerEndpoint { get; set; } = null!;
    public string Locale { get; set; } = null!;
    public int TimezoneOffset { get; set; }

    public string SecurityToken { get; set; } = null!;
    public string Cst { get; set; } = null!;
}