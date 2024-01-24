using Boiler.Mobile.Models.Thermometer;
using Fluxor;

namespace Boiler.Mobile.Components.Pages.Thermometer.List;

public class State
{
    public List<ThermometerModel> Thermometers { get; set; } = new List<ThermometerModel>();
    public bool Loading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
}
public class ThermometerFeature : Feature<State>
{
    public override string GetName() => "ThermometerList";

    protected override State GetInitialState() =>
        new State();
}