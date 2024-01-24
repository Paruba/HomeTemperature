using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using IDispatcher = Fluxor.IDispatcher;

namespace Boiler.Mobile.Components.Pages.Thermometer.List;

public class Effects
{
    private readonly ApiClient _apiClient;

    public Effects(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    [EffectMethod]
    public async Task GetThermometers(Actions.GetThermometers action, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiClient.GetFromJsonAsync<PageContainer<ThermometerModel>>($"{Boiler.Mobile.Shared.Routes.Thermometer.ThermometerBase}?searchValue={action.SearchValue}&skip={action.Skip}&take={action.Take}");
            if (response != null)
                dispatcher.Dispatch(new Actions.SetThermometers(response.Items.ToList()));
            else
                dispatcher.Dispatch(new Actions.Error("Nepodařilo se získat termostaty."));
        }
        catch (Exception)
        {
            dispatcher.Dispatch(new Actions.Error("Chyba backendu."));
        }
    }
}
