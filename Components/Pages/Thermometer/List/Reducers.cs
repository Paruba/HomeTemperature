
using Fluxor;

namespace Boiler.Mobile.Components.Pages.Thermometer.List;

public class Reducers
{
    [ReducerMethod]
    public static State ClearState(State state, Actions.ClearState action)
            => new State();

    [ReducerMethod]
    public static State SetLoading(State state, Actions.GetThermometers action)
        => new State() { Loading = true };

    [ReducerMethod]
    public static State UnsetLoading(State state, Actions.SetThermometers action)
        => new State() { Loading = false, Thermometers = action.Thermometers };

    [ReducerMethod]
    public static State OnError(State state, Actions.Error action)
        => new State() { Loading = false, ErrorMessage = action.ErrorMessage };
}
