namespace Ig.Api.Client.Model;

public class AuthenticationRequest
{
    public string Identifier { get; set; } = null!;
    public string Password { get; set; } = null!;
}

