namespace Boiler.Mobile.Framework.Auth;

public class ClientAuthroizationStateNotifier
{
    public delegate void AuthenticationStateChangedEventHandler(object sender, EventArgs e);

    public event AuthenticationStateChangedEventHandler AuthenticationStateChanged;

    public void NotifyAuthorizationStateChanged()
    {
        AuthenticationStateChanged?.Invoke(this, new EventArgs());
    }
}