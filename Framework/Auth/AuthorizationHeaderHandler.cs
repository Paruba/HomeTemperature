using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Boiler.Mobile.Framework.Auth;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthorizationHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = await _localStorage.GetItemAsStringAsync("token");

        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeader.Replace("Bearer ", ""));
        }

        return await base.SendAsync(request, cancellationToken);
    }
}