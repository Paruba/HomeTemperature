using Boiler.Mobile.Framework.Http;
using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace Boiler.Mobile.Components.Pages.Thermometer.List;

public partial class List
{
    [Inject] public IState<State> PageState { get; set; }
    [Inject] public Fluxor.IDispatcher Dispatcher { get; set; }
    [Inject] public IActionSubscriber ActionSubscriber { get; set; }
    [Inject] public NavigationManager navigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public ApiClient HttpApiClient { get; set; }
    private MudTable<ThermometerModel> _table;
    private MudPagination _pagination;
    public int CurrentPage { get; set; }
    private int _tablePage;
    private string search = string.Empty;
    private DotNetObjectReference<List> _pageContainerObjRef;
    private string _searchText
    {
        get { return search; }
        set
        {
            search = value;
            Dispatcher.Dispatch(new Actions.GetThermometers(value));
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override void Dispose(bool disposing)
    {
        Dispatcher.Dispatch(new Actions.ClearState());
        ActionSubscriber.UnsubscribeFromAllActions(this);
        base.Dispose(disposing);
    }

    private void OnRowClick(TableRowClickEventArgs<ThermometerModel> args)
    {
        navigationManager.NavigateTo($"/temperature?thermometerId={args.Item.Id}");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        _pageContainerObjRef = DotNetObjectReference.Create(this);
        await JsRuntime.InvokeVoidAsync("setConteinerDotnetHelper", _pageContainerObjRef);
        if (firstRender)
        {
            var page = await GetPageCookie();
            if (!string.IsNullOrEmpty(page) && int.TryParse(page, out int pageNumber))
            {
                CurrentPage = pageNumber;
                _pagination.Selected = pageNumber + 1;
                StateHasChanged();
            }
        }
    }

    private async Task<TableData<ThermometerModel>> ServerReload(TableState state)
    {
        try { 
            var response = await HttpApiClient.GetFromJsonAsync<PageContainer<ThermometerModel>>($"{Boiler.Mobile.Shared.Routes.Thermometer.ThermometerBase}?searchValue={search}");
            IEnumerable<ThermometerModel> data = response.Items;
            data = data.Where(element =>
            {
                if (string.IsNullOrWhiteSpace(search))
                    return true;
                if (element.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    return true;
                return false;
            }).ToList();

            switch (state.SortLabel)
            {
                case "created_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Name);
                    break;
            }
            var pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
            return new TableData<ThermometerModel>() { TotalItems = response.TotalCount, Items = pagedData };
        }
        catch (Exception)
        {
            Dispatcher.Dispatch(new Actions.Error("Nepodařilo se získat data."));
        }
        return new TableData<ThermometerModel>() { TotalItems = 0, Items = new List<ThermometerModel>()};
    }

    private async Task SetPageCookie(int pageValue)
    {
        await JsRuntime.InvokeVoidAsync("cookieFunctions.setCookie", "thermometer-Page", pageValue.ToString(), 60);
    }

    private async Task<string> GetPageCookie()
    {
        return await JsRuntime.InvokeAsync<string>("cookieFunctions.getCookie", "thermometer-Page");
    }

    private void PageChanged(int page)
    {
        var tempPage = page - 1;
        CurrentPage = tempPage;
        _ = SetPageCookie(tempPage);
    }
}