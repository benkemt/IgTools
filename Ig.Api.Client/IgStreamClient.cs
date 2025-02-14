using com.lightstreamer.client;

namespace Ig.Api.Client;



public interface IClientListener : ClientListener
{
}

public interface IIgStreamClient
{
    void LoginAsync( IClientListener clientListener);
    void Disconnect();
}

public class IgStreamClient(IAuthService authService) : IIgStreamClient
{
    private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    private LightstreamerClient? _lightStreamerClient = null;

    public  void LoginAsync( IClientListener clientListener)
    {
        var lightStreamServerAddress = _authService.GetLightStreamServerAddress();
        _lightStreamerClient = new LightstreamerClient(lightStreamServerAddress, "DEFAULT")
        {
            connectionDetails =
            {
                User = _authService.GetAccountId(),
                Password = $"CST-{_authService.GetCstToken()}|XST-{_authService.GetSecurityToken()}"
            }
        };

        _lightStreamerClient.addListener(clientListener);

        _lightStreamerClient.connect();
    }

    public void Disconnect()
    {
        if (_lightStreamerClient != null)
        {
            _lightStreamerClient.disconnect();
            _lightStreamerClient = null;
        }
    }
}