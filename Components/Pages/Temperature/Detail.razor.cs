using Boiler.Mobile.Framework.Auth;
using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Globalization;
using System.Net.Http.Json;

namespace Boiler.Mobile.Components.Pages.Temperature;

public partial class Detail
{
    [Inject] public IState<State> PageState { get; set; }
    [Inject] public Fluxor.IDispatcher Dispatcher { get; set; }
    [Inject] public IActionSubscriber ActionSubscriber { get; set; }
    [Inject] public IConfiguration Configuration { get; set; }
    [Inject] public ApiClient HttpApiClient { get; set; }
    [Inject] public IAccessTokenProvider TokenProvider { get; set; }
    [Parameter]
    [SupplyParameterFromQuery]
    public string ThermometerId { get; set; }

    public int CurrentPage { get; set; }

    private MudTable<TemperatureModel> _table;
    private MudPagination _pagination;

    public List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Teplota", Data = new double[] { } },
    };
    public string[] XAxisLabels = { };
    public ChartOptions Options = new ChartOptions();
    private HubConnection hubConnection;

    protected override async Task OnInitializedAsync()
    {
        Dispatcher.Dispatch(new Actions.GetCurrentValue(ThermometerId));
        var signalRUrl = $"{Configuration.GetSection("UrlAddress").Value}temperatureHub";
        hubConnection = new HubConnectionBuilder()
            .WithUrl(signalRUrl, options =>
            {
                options.AccessTokenProvider = async () => await TokenProvider.GetTokenAsync();
            })
            .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Debug))
            .WithAutomaticReconnect()
            .Build();
        
        hubConnection.On<string>("ReceiveTemperature", (value) =>
        {
            Dispatcher.Dispatch(new Actions.SetCurrentValue(value));
        });
        
        _ = Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
            await hubConnection.StartAsync());
        });
        await base.OnInitializedAsync();
    }

    protected override void Dispose(bool disposing)
    {
        Dispatcher.Dispatch(new Actions.ClearState());
        ActionSubscriber.UnsubscribeFromAllActions(this);
        if (hubConnection is not null)
        {
            hubConnection.StopAsync();
            hubConnection.DisposeAsync();
        }
        base.Dispose(disposing);
    }

    private async Task<TableData<TemperatureModel>> ServerReload(TableState state)
    {
        try
        {
            var response = await HttpApiClient.GetFromJsonAsync<PageContainer<TemperatureModel>>($"{Boiler.Mobile.Shared.Routes.Temperatures.Base}?thermometerId={ThermometerId}&skip={(state.Page * state.PageSize)}&take={state.PageSize}");
            IEnumerable<TemperatureModel> data = response.Items;
            XAxisLabels = response.Items.Select(x => x.Time.Value.Minute.ToString()).ToArray();
            double[] doubleArray = response.Items.Select(s => double.Parse(s.Value, CultureInfo.InvariantCulture)).ToArray();
            Series.Clear();
            Series.Add(new ChartSeries() { Name = "Teplota", Data = doubleArray });
            switch (state.SortLabel)
            {
                case "created_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Time);
                    break;
            }
            StateHasChanged();
            return new TableData<TemperatureModel>() { TotalItems = response.TotalCount, Items = response.Items };
        }
        catch (Exception)
        {
            Dispatcher.Dispatch(new Actions.Error("Nepodařilo se získat data."));
        }
        return new TableData<TemperatureModel>() { TotalItems = 0, Items = new List<TemperatureModel>() };
    }

    private void PageChanged(int page)
    {
        var tempPage = page - 1;
        CurrentPage = tempPage;
    }
}
