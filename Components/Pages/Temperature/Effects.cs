using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;
using Fluxor;
using System.Net.Http.Json;
using IDispatcher = Fluxor.IDispatcher;

namespace Boiler.Mobile.Components.Pages.Temperature;

public class Effects
{
    private readonly ApiClient _apiClient;

    public Effects(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    [EffectMethod]
    public async Task GetCurrentTemperature(Actions.GetCurrentValue action, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiClient.GetFromJsonAsync<TemperatureModel>($"{Boiler.Mobile.Shared.Routes.Temperatures.Current}?thermometerId={action.ThermometerId}");
            if (response != null)
                dispatcher.Dispatch(new Actions.SetCurrentValue(response.Value));
            else
                dispatcher.Dispatch(new Actions.Error("Nepodařilo se získat současnou teplotu."));
        }
        catch (Exception)
        {
            dispatcher.Dispatch(new Actions.Error("Chyba backendu."));
        }
    }

    [EffectMethod]
    public async Task GetTemperatures(Actions.GetTemperatures action, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiClient.GetFromJsonAsync<PageContainer<TemperatureModel>>($"{Boiler.Mobile.Shared.Routes.Temperatures.Base}?thermometerId={action.ThermometerId}&skip={action.Skip}&take={action.Take}");
            dispatcher.Dispatch(new Actions.SetTemperatures(response));
        } catch (Exception)
        {
            dispatcher.Dispatch(new Actions.Error("Server error"));
        }
    }
}
