using Boiler.Mobile.Models;
using Boiler.Mobile.Models.Thermometer;

namespace Boiler.Mobile.Components.Pages.Temperature;

public class Actions
{
    public class GetCurrentValue {
        public string ThermometerId { get; private set; }
        public GetCurrentValue(string thermometerId) { 
            ThermometerId = thermometerId;
        }
    }
    public class SetCurrentValue
    {
        public string CurrentValue { get; private set; }
        public SetCurrentValue(string currentValue)
        {
            CurrentValue = currentValue;
        }
    }

    public class GetTemperatures
    {
        public string ThermometerId { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public GetTemperatures(string thermometerId, int skip = 0, int take = 20)
        {
            ThermometerId = thermometerId;
            Skip = skip;
            Take = take;
        }
    }

    public class SetTemperatures
    {
        public PageContainer<TemperatureModel> Temperatures { get; private set;}
        public SetTemperatures(PageContainer<TemperatureModel> temperatures)
        {
            Temperatures = temperatures;
        }
    }

    public class Error
    {
        public string? ErrorMessage { get; set; }
        public Error(string? errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    public class ClearState { }
}
