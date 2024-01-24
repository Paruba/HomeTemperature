using Blazored.LocalStorage;

namespace Boiler.Mobile.Framework.Auth;

public class AccessTokenProvider : IAccessTokenProvider
{
    private ILocalStorageService localStorageService { get; set; }
    public AccessTokenProvider(ILocalStorageService localStorage)
    {
        localStorageService = localStorage;
    }

    public async Task<string> GetTokenAsync()
    {
        try { 
            var token = await localStorageService.GetItemAsStringAsync("token");
            return token;
        } catch (Exception)
        {
            return "";
        }
    }
}
