using Fluxor;

namespace Boiler.Mobile.Components.Pages.Temperature;

public class Reducers
{
    [ReducerMethod]
    public static State ClearState(State state, Actions.ClearState action)
            => new State();

    [ReducerMethod]
    public static State SetLoading(State state, Actions.GetCurrentValue action)
        => new State() { Loading = true, CurrentTemperature = state.CurrentTemperature, TemperatureData = state.TemperatureData};

    [ReducerMethod]
    public static State UnsetLoading(State state, Actions.SetCurrentValue action)
        => new State() { Loading = false, CurrentTemperature = action.CurrentValue, TemperatureData = state.TemperatureData };

    [ReducerMethod]
    public static State SetTemperatureData(State state, Actions.SetTemperatures action)
        => new State() { Loading = state.Loading, CurrentTemperature = state.CurrentTemperature, TemperatureData = action.Temperatures };

    [ReducerMethod]
    public static State OnError(State state, Actions.Error action)
        => new State() { Loading = false, ErrorMessage = action.ErrorMessage };
}
