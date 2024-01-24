using Blazored.LocalStorage;
using Boiler.Mobile.Framework.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Boiler.Mobile.Framework.Auth;

public class ClientAuthorizationStateProvider : AuthenticationStateProvider
{
    private readonly ApiClient _http;
    private readonly ILocalStorageService _localStorage;

    public ClientAuthorizationStateProvider(ApiClient http, ILocalStorageService localStorage, ClientAuthroizationStateNotifier clientAuthroizationStateNotifier)
    {
        _http = http;
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        clientAuthroizationStateNotifier.AuthenticationStateChanged += HandleAuthenticationStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorage.GetItemAsStringAsync("token");

        var identity = new ClaimsIdentity();
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authToken))
        {
            try
            {
                authToken = authToken.Replace("\"", "");
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurtyToken = handler.ReadJwtToken(authToken);
                var identityClaims = new List<Claim> { };
                identityClaims.AddRange(jwtSecurtyToken.Claims);
                identity = new ClaimsIdentity(identityClaims, "Bearer");
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);
            }
            catch
            {
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }


    private void HandleAuthenticationStateChanged(object sender, EventArgs e)
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}