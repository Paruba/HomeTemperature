using Blazored.LocalStorage;
using Boiler.Mobile.Framework;
using Boiler.Mobile.Framework.Auth;
using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Framework.Logging;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Reflection;

namespace Boiler.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            var _assembly = Assembly.GetExecutingAssembly();
            var _assemblyName = _assembly.GetName().Name;
            #region Config
            using var stream = _assembly.GetManifestResourceStream($"{_assemblyName}.appSettings.json");
            
            var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
            builder.Configuration.AddConfiguration(config);
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            #endregion
            #region Http
            builder.Services.AddTransient<AuthorizationHeaderHandler>();
            builder.Services.AddScoped(sp => {
                var serverAddress = builder.Configuration.GetSection("UrlAddress").Value;
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var localStorageService = sp.GetRequiredService<ILocalStorageService>();
                var authTokenHandler = new AuthorizationHeaderHandler(localStorageService);
                authTokenHandler.InnerHandler = clientHandler;
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                ApiClient _http = new ApiClient(authTokenHandler) { BaseAddress = new Uri(serverAddress) };
                _http.OnUnauthorizedResponse += () => navigationManager.NavigateTo(Boiler.Mobile.Shared.Routes.User.Logout);
                return _http;
            });
            #endregion
            #region Auth
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<ClientAuthroizationStateNotifier>();
            builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthorizationStateProvider>();
            #endregion
            #region dependency
            var servicesBuilder = new ApiServicesBuilder();
            servicesBuilder.ConfigureServices(builder.Services, new string[] { _assemblyName });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMudServices();
            builder.Services.AddHttpContextAccessor();
            #endregion
            #region Fluxor
            builder.Services.AddFluxor(opts =>
            {
                opts.ScanAssemblies(typeof(App).Assembly);
                opts.AddMiddleware<LoggingMiddleware>();
                opts.UseReduxDevTools();
            });
            #endregion
            

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
