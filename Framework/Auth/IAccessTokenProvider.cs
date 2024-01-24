namespace Boiler.Mobile.Framework.Auth;

public interface IAccessTokenProvider : IDependency
{
    public Task<string> GetTokenAsync();
}
