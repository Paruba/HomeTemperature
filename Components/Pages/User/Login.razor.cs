using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Models.Identity;
using System.Net.Http.Json;
using Boiler.Mobile.Framework.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace Boiler.Mobile.Components.Pages.User;

public partial class Login
{
    [Inject] public ApiClient HttpApiClient { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    private LoginModel _loginForm = new LoginModel() { };
    private EditContext _editContext;
    private string errorMessage = string.Empty;
    private string returnUrl = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _editContext = new EditContext(_loginForm);
        await base.OnInitializedAsync();
    }

    private async Task OnValidSubmit(EditContext editContext)
    {
        try { 
            var response = await HttpApiClient.PostFromJsonAsync<LoginResponseModel>(Boiler.Mobile.Shared.Routes.User.Login, _loginForm);
            if (response != null)
            {
                errorMessage = string.Empty;
                await LocalStorage.SetItemAsStringAsync("token", response.Token);
                await AuthStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo(Boiler.Mobile.Shared.Routes.Thermometer.Thermometers, true);
            }
            else
                errorMessage = "Nesprávný login.";
        } catch (Exception)
        {
            errorMessage = "Nesprávný login.";
        }
    }
}