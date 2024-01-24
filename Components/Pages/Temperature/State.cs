using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;
using Fluxor;

namespace Boiler.Mobile.Components.Pages.Temperature;

public class State
{
    public string CurrentTemperature { get; set; }
    public PageContainer<TemperatureModel> TemperatureData { get; set; } = new PageContainer<TemperatureModel>();
    public bool Loading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
}
public class TemperatureFeature : Feature<State>
{
    public override string GetName() => "TemperatureDetail";

    protected override State GetInitialState() =>
        new State();
}
