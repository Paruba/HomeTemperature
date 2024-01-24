using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Boiler.Mobile.Components.Pages.User;

public partial class Logout
{
    [Inject] ILocalStorageService localStorage { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    protected override async Task OnInitializedAsync()
    {
        localStorage.RemoveItemAsync("token");
        await AuthStateProvider.GetAuthenticationStateAsync();
        NavigationManager.NavigateTo("/login");
        await base.OnInitializedAsync();
    }
}
